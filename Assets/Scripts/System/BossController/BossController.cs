using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    StateMachine stateMachine;
    BossActionController actionController;
    BossPhaseController phaseController;
    HealthbarManager healthbarManager;
    Animator animator;

    [Header("Base")]
    [SerializeField] float currentHealth = 100;
    [SerializeField] public Transform target = null;
    [SerializeField] string state = "";
    [SerializeField] float staggerCoef = 0.1f;

    [Header("Attack stats")]
    [SerializeField] public float recoverTimer = 0f;
    [SerializeField] float staggerValue = 0f;
    [SerializeField] float windUpTimer = 0f;
    [SerializeField] float distanceToTarget = 0f;


    void Start()
    {
        animator = GetComponent<Animator>();
        healthbarManager = GetComponent<HealthbarManager>();
        actionController = GetComponent<BossActionController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initStates();
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

    public float getDistance()
    {
        return Vector2.Distance(target.position, transform.position);
    }

    public void setRecoverTimer(float value)
    {
        recoverTimer = value;
    }

    void initStates()
    {
        stateMachine = new StateMachine();

        //States
        BaseState idleState = new BossIdleState(animator);
        BaseState actionState = new BossActionState(this, animator, actionController);
        BaseState recoverState = new BossRecoverState(this, animator);
        BaseState staggerState = new BossStaggerState(animator);
        BaseState deadState = new BossDeadState(animator);

        
        Any(idleState, new FuncPredicate(() => target == null && currentHealth > 0 && recoverTimer <= 0));
        Any(deadState, new FuncPredicate(() => currentHealth <= 0));

        At(idleState, actionState, new FuncPredicate(() => target != null && currentHealth > 0));
        At(actionState, recoverState, new FuncPredicate(() => recoverTimer > 0));
        At(recoverState, actionState, new FuncPredicate(() => recoverTimer <= 0));
        At(recoverState, idleState, new FuncPredicate(() => target == null && recoverTimer <= 0));
        At(recoverState, staggerState, new FuncPredicate(() => staggerValue > 1f)); //todo value
        At(staggerState, actionState, new FuncPredicate(() => staggerValue <= 0f));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
}
