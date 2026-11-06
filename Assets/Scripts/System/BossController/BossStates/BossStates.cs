using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BossIdleState : BaseState
{
    public BossIdleState(Animator animator) : base(animator)
    {
        
    }

    public override void onEnter()
    {
        animator.Play("Idle");
    }
}

public class BossChaseState : BaseState
{
    NavMeshAgent agent;
    BossController controller;
    float cheseDistance = 5;
    public BossChaseState(BossController bossController, Animator animator, NavMeshAgent agent, float chaseDistance) : base(animator)
    {
        this.agent = agent;
        this.controller = bossController;
        this.cheseDistance = chaseDistance;
    }

    public override void onEnter()
    {
        // animator.Play("Chase");
        agent.enabled = true;
        agent.destination = controller.target.position;
    }

    public override void onExit()
    {
        agent.enabled = false;
    }
}

public class BossActionState : BaseState
{
    BossActionController actionController;
    BossController controller;
    BasicTimer windUpTimer;
    BossAction currentAction = null;
    public BossActionState(BossController controller, Animator animator, BossActionController actionController) : base(animator)
    {
        this.actionController = actionController;
        this.controller = controller;

        windUpTimer = new BasicTimer();
    }

    public override void onEnter()
    {
        controller.activateRotator();
        windUpTimer.Reset();
        currentAction = actionController.getActionFor(controller.getDistance());
        // Debug.Log($"Attack selected {currentAction.data.actionName}");
        animator.Play(currentAction.data.windUpName);
    }

    public override void update()
    {
        if (currentAction != null && windUpTimer.IsFinished(currentAction.data.windUpDuration))
        {
            controller.deactivateRotator();
            controller.setRecoverTimer(currentAction.data.recoverDuration);
            performAction();
            
        }
        windUpTimer.Tick();
    }

    void performAction()
    {
        currentAction.performAction();
    }

    public override void onExit()
    {
        currentAction = null;
    }
}

public class BossRecoverState : BaseState
{
    BossController controller;
    public BossRecoverState(BossController controller, Animator animator) : base(animator)
    {
        this.controller = controller;
    }

    public override void onEnter()
    {
        animator.Play("Recover");
    }

    public override void update()
    {
        controller.recoverTimer -= Time.deltaTime;
    }

    public override void onExit()
    {
        controller.recoverTimer = 0;
    }
}

public class BossStaggerState : BaseState
{
    public BossStaggerState(Animator animator) : base(animator)
    {
        
    }
}

public class BossDeadState : BaseState
{
    BossController controller;
    public BossDeadState(BossController bossController, Animator animator) : base(animator)
    {
        this.controller = bossController;
    }

    public override void onEnter()
    {
        controller.die();
        animator.Play("Dying");
    }
}


public class BasicTimer
{
    private float elapsed;

    public bool IsFinished(float duration)
    {
        return elapsed >= duration;
    }

    public void Tick()
    {
        elapsed += Time.deltaTime;
    }

    public void Reset()
    {
        elapsed = 0f;
    }
}