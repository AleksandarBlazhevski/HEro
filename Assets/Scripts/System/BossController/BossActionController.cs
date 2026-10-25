using System.Collections.Generic;
using UnityEngine;

public class BossActionController
{
    private HashSet<BossAction> actionSet;

    public BossActionController(IEnumerable<BossAction> actions)
    {
        actionSet = new HashSet<BossAction>(actions);
    }

    public void updateCooldown()
    {
        foreach(var action in actionSet)
        {
            action.updateCooldown();
        }
    }

    public void addAction(BossAction action)
    {
        actionSet.Add(action);
    }

    public void addActions(IEnumerable<BossAction> actions)
    {
        actionSet = new HashSet<BossAction>(actions);
    }

    public void clear()
    {
        actionSet.Clear();
    }

    public BossAction getActionFor(float distance)
    {
        BossAction selectedAction = null;
        float bestScore = 0f;

        foreach (BossAction action in actionSet)
        {
            float score = action.calculateScore(distance);
            if(score > bestScore)
            {
                selectedAction = action;
            }
        }

        return selectedAction;
    }


}