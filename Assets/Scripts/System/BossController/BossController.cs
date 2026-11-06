using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    StateMachine stateMachine;
    BossActionController actionController;
    BossPhaseController phaseController;
    HealthbarManager healthbarManager;
    SpriteRotator spriteRotator;
    Animator animator;
    NavMeshAgent agent;

    [Header("Base")]
    // [SerializeField] float currentHealth = 100;
    [SerializeField] public Transform target = null;
    [SerializeField] string state = "";
    [SerializeField] float staggerCoef = 0.1f;

    [Header("Attack stats")]
    [SerializeField] public float recoverTimer = 0f;
    [SerializeField] float staggerValue = 0f;
    [SerializeField] float distanceToTarget = 0f;
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] Transform dummy;


    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        healthbarManager = GetComponentInChildren<HealthbarManager>();
        spriteRotator = GetComponentInChildren<SpriteRotator>();
        actionController = GetComponent<BossActionController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initStates();
        
    }

    void Update()
    {
        if(dummy) agent.destination = dummy.position;
        if (spriteRotator.enabled)
        {
            lookAtTarget();
        }
    }

    void FixedUpdate()
    {
        stateMachine.Update();
        actionController.updateCooldown();
        
        if(target != null) distanceToTarget = Vector2.Distance(transform.position, target.position);

        state = stateMachine.getCurrentStateName();
        if(staggerValue > 0)
        {
            staggerValue -= (Time.deltaTime * staggerCoef);
            if(staggerValue < 0) staggerValue = 0;
        }
        
    }

    public void takeDamage(float damage)
    {
        healthbarManager.takeDamage(damage);
    }

    public float getDistance()
    {
        return Vector2.Distance(target.position, transform.position);
    }

    public void setRecoverTimer(float value)
    {
        recoverTimer = value;
    }

    public void lookAtTarget()
    {
        Vector2 direction = target.position - transform.position;
        spriteRotator.lookAtDirection(direction);
    }

    public void deactivateRotator()
    {
        spriteRotator.enabled = false;
    }

    public void activateRotator()
    {
        spriteRotator.enabled = true;
    }

    public void die()
    {
        deactivateRotator();
        gameObject.tag = "Dead";
    }

    void initStates()
    {
        stateMachine = new StateMachine();

        //States
        BaseState idleState = new BossIdleState(animator);
        BaseState chaseState = new BossChaseState(this, animator, agent, chaseDistance);
        BaseState actionState = new BossActionState(this, animator, actionController);
        BaseState recoverState = new BossRecoverState(this, animator);
        BaseState staggerState = new BossStaggerState(animator);
        BaseState deadState = new BossDeadState(this, animator);

        
        Any(idleState, new FuncPredicate(() => target == null && healthbarManager.CurrentHealth > 0 && recoverTimer <= 0));
        Any(deadState, new FuncPredicate(() => healthbarManager.CurrentHealth <= 0));

        At(idleState, actionState, new FuncPredicate(() => target != null && healthbarManager.CurrentHealth > 0));
        At(actionState, recoverState, new FuncPredicate(() => recoverTimer > 0));
        At(actionState, chaseState, new FuncPredicate(() => target != null && recoverTimer <= 0 && chaseDistance < distanceToTarget));
        At(chaseState, actionState, new FuncPredicate(() => target != null && recoverTimer <= 0 && chaseDistance > distanceToTarget));
        At(recoverState, actionState, new FuncPredicate(() => recoverTimer <= 0));
        At(recoverState, idleState, new FuncPredicate(() => target == null && recoverTimer <= 0));
        At(recoverState, staggerState, new FuncPredicate(() => staggerValue > 1f)); //todo value
        At(staggerState, actionState, new FuncPredicate(() => staggerValue <= 0f));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
}
