using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_Actions : Enemy_Actions
{
    public RangedSkeleton_ThrowProjectile throwProj;
    public Enemy_Refs eRefs;

    public override void StartChecks() {
        throwProj.StartCheck();
    }

    public override void StopActions() {
        throwProj.StopAllCoroutines();
        throwProj.enabled = false;
    }
}
