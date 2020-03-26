using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton_Actions : Enemy_Actions
{
    public ShieldSkeleton_ShieldBash shieldBash;
    public ShieldSkeleton_ShieldUp shieldUp;
    public Enemy_Movement_Defend movement_Defend;
    public Enemy_Refs eRefs;

    public override void StartChecks() {
        shieldBash.StartCheck();
        movement_Defend.StartDefendingDefendee();
    }

    public override void StopActions() {
        shieldBash.StopAction();
    }
}
