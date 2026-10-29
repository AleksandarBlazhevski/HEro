using System;
using Unity.VisualScripting;
using UnityEngine;

//Maybe change to scripatable object?
public class BossAction
{
    [Header("Conditions")]
    [SerializeField] float minDistance = 0f;
    [SerializeField] float maxDistance = float.MaxValue;
    [SerializeField] public float cooldown = 2f;
    [SerializeField] public float cooldownTimer = 0;
    [SerializeField] public float recoverDuration = 2f;
    [SerializeField] public float windUpDuration = 2f;
    [SerializeField] public float actionDuration = 2f;


    [Header("Preferences")]
    [SerializeField] protected float weigth = 10f;
    [SerializeField] float preferredRange = 2f;

    private Animator animator;
    public string windup {get; private set;}
    public string action {get; private set;}


    public BossAction(Animator animator, string windup, string action)
    {
        this.animator = animator;
        this.windup = windup;
        this.action = action;
    }

    public float calculateScore(float distance)
    {
        if(cooldownTimer > 0){return 0f;}

        float rangeError = Mathf.Abs(distance - preferredRange);
        float score = 1f / (1f + rangeError);

        return weigth * score;
    }

    public void updateCooldown()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer < 0) cooldownTimer = 0;
        }
    }

    public void startWindUp()
    {
        animator.Play("Projectile1_wind_up");
    }

    public void startAction()
    {
        Debug.Log("Action started");
    }
}