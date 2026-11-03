using UnityEngine;
using System.Collections;

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
        windUpTimer.Reset();
        currentAction = actionController.getActionFor(controller.getDistance());
        Debug.Log($"Attack selected {currentAction.data.actionName}");
        animator.Play(currentAction.data.windUpName);
    }

    public override void update()
    {
        if (currentAction != null && windUpTimer.IsFinished(currentAction.data.windUpDuration))
        {
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