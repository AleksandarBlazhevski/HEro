using UnityEngine;

public abstract class BaseState: IState
{
    protected readonly Animator animator;
    protected BaseState(Animator animator)
    {
        this.animator = animator;
    }
    public virtual void onEnter() { }
    public virtual void update(){}
    
    public virtual void fixedUpdate(){}
    public virtual void onExit()
    {
    }
}