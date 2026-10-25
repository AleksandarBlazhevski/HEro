using UnityEngine;

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
    public BossActionState(BossController controller, Animator animator, BossActionController actionController) : base(animator)
    {
        this.actionController = actionController;
        this.controller = controller;
    }

    public override void onEnter()
    {
        BossAction action = actionController.getActionFor(controller.getDistance());
        controller.performAction(action);
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
