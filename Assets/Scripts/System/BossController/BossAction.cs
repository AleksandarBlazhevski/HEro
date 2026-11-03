using System;
using Unity.VisualScripting;
using UnityEngine;

public class BossAction
{
    [SerializeField] public float cooldownTimer = 0;
    [SerializeField] public BossActionData data;
    Animator animator;
    BossActionController actionController;

    public BossAction(BossActionData data, Animator animator, BossActionController actionController)
    {
        this.data = data;
        this.animator = animator;
        this.actionController = actionController;
    }


    //TODO
    public float calculateScore(float distance)
    {
        if(cooldownTimer > 0){return 0f;}
        
        float rangeError = Mathf.Abs(distance - data.preferredRange);
        float score = 1f / (1f + rangeError);

        Debug.Log($"[{data.attackName}] Error distance: {rangeError}. Score: {score}");

        return data.weigth * score;
    }

    public void updateCooldown()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer < 0) cooldownTimer = 0;
        }
    }

    public void performAction()
    {
        animator.Play(data.actionName);
        cooldownTimer = data.cooldown;
        if(data.projectile != null)
        {
            actionController.spawnProjectile(data.projectile);
        }
    }

}