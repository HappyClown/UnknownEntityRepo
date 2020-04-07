using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Chase : MonoBehaviour
{
    public Enemy_Refs eRefs;
    bool stateStarted;

    // Set movement target to Player.
    public void AssignChaseTarget() {
        if (eRefs.eFollowPath.target != eRefs.plyrTrans) {
            eRefs.eFollowPath.target = eRefs.plyrTrans;
        }
    }
}
