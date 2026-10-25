using System;
using UnityEngine;

//Maybe change to scripatable object
public class BossAction
{
    [Header("Conditions")]
    [SerializeField] float minDistance = 0f;
    [SerializeField] float maxDistance = float.MaxValue;
    [SerializeField] public float cooldown = 2f;
    [SerializeField] public float cooldownTimer = 0;
    [SerializeField] public float recoverTime = 2f;
    [SerializeField] public float duration = 2f;


    [Header("Preferences")]
    [SerializeField] protected float weigth = 10f;
    [SerializeField] float preferredRange = 2f;

    public float calculateScore(float distance)
    {
        if(cooldownTimer > 0){return 0f;}

        float rangeError = Mathf.Abs(distance - preferredRange);
        float score = 1f / (1f + rangeError);

        return weigth * score;
    }

    public virtual void perform()
    {
        //TODO
        cooldownTimer = cooldown;
    }

    public void updateCooldown()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer < 0) cooldownTimer = 0;
        }
    }
}

public class BossActionTest1 : BossAction
{
    public BossActionTest1() : base()
    {

    }
    public override void perform()
    {
        base.perform();
        Debug.Log("[BOSSACTION] test1");
    }
}

public class BossActionTest2 : BossAction
{
    public BossActionTest2() : base()
    {
        weigth = 20f;
        cooldown = 8f;
    }
    public override void perform()
    {
        base.perform();
        Debug.Log("[BOSSACTION] test2");
    }
}