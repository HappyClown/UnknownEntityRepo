using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_Death : Enemy_SpecificDeath
{
    public Enemy_AllyToDefend eAllyToDef;
    public override void PlayOnDeath() {
        eAllyToDef.FreeUpDefender();
    }
}
