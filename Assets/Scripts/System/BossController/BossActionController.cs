using System;
using System.Collections.Generic;
using UnityEngine;

public class BossActionController : MonoBehaviour
{
    List<BossAction> actionList = new List<BossAction>();
    [SerializeField] public List<BossActionData> actionDataList;
    [SerializeField] public Transform firePoint;

    Animator animator;
    BossController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<BossController>();

        foreach(var data in actionDataList){
            actionList.Add(new BossAction(data, animator, this));
        }
    }

    public void updateCooldown()
    {
        foreach(var action in actionList)
        {
            action.updateCooldown();
        }
    }

    public void addAction(BossAction action)
    {
        actionList.Add(action);
    }

    public void addActions(IEnumerable<BossAction> actions)
    {
        actionList = new List<BossAction>(actions);
    }

    public void clear()
    {
        actionList.Clear();
    }

    public BossAction getActionFor(float distance)
    {
        BossAction selectedAction = null;
        float bestScore = 0f;

        foreach (BossAction action in actionList)
        {
            float score = action.calculateScore(distance);
            if(score > bestScore)
            {
                selectedAction = action;
                bestScore = score;
            }
        }
        Debug.Log($"Best score: {bestScore}");
        return selectedAction;
    }

    internal void spawnProjectile(GameObject projectile)
    {
        Vector2 direction =
            (controller.target.position - transform.position).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Instantiate(
            projectile,
            firePoint.position,
            transform.rotation
        );

    }
}