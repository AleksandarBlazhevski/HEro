using UnityEngine;
using System.Collections;

public class BossIdleState : BaseState
{
    public BossIdleState(Animator animator) : base(animator)
    {
        
    }
}

public class BossActionState : BaseState
{
    BossActionController actionController;
    BossController controller;
    StateTimer windUpTimer;
    BossAction currentAction = null;
    public BossActionState(BossController controller, Animator animator, BossActionController actionController) : base(animator)
    {
        this.actionController = actionController;
        this.controller = controller;

        windUpTimer = new StateTimer();
    }

    public override void onEnter()
    {
        currentAction = actionController.getActionFor(controller.getDistance());
        windUpTimer.Reset();
        animator.Play(currentAction.windup);
    }

    public override void update()
    {
        if (currentAction != null && windUpTimer.IsFinished(currentAction.windUpDuration))
        {
            animator.Play(currentAction.action);
            // controller.setRecoverTimer(currentAction.recoverDuration);
        }
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
        Debug.Log("Boss Recovering...");
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
    public BossDeadState(Animator animator) : base(animator)
    {
        
    }
}


public class StateTimer
{
    private float elapsed;

    public bool IsFinished(float duration)
    {
        elapsed += Time.deltaTime;
        return elapsed >= duration;
    }

    public void Reset()
    {
        elapsed = 0f;
    }
}