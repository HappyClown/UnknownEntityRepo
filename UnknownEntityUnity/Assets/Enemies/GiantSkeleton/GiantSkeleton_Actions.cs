using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSkeleton_Actions : Enemy_Actions
{
    [Header ("Script References")]
    public Enemy_Refs eRefs;
    public Enemy_Movement_Chase movement_Chase;
    public Enemy_Wait enemy_Wait;
    public GiantSkeleton_SlashAttack giantSkel_SlashAtk;
    public GiantSkeleton_GroundAttack giantSkel_GroundAtk;
    [Header ("To-set Variables")]
    public bool debugs;
    public float chaseDistance;

    [Header ("Read Only")]
    public string curState;
    public bool updateState = true;
    bool stateStarted = false;
    public float ChaseDistanceSqr {
        get {
            return chaseDistance * chaseDistance;
            }
    }
    
    public override void StartChecks() {
        // Create a new state machine for this enemy. 
        // The "checkingAggro" state should be implemented in the state machine only if needed.
        //brain = new FSM();
        brain = this.gameObject.AddComponent<FSM>();
        // Set initial state.
        brain.SetActiveState(ChaseTarget);
    }
    public override void StopActions() {
        giantSkel_SlashAtk.StopSlashAttack();
        giantSkel_GroundAtk.StopGroundAttack();
    }
    public override void StopStateUpdates () {
        brain.SetActiveState(Neutral);
        updateState = false;
    }
    public override void ResumeStateUpdates () {
        eRefs.eFollowPath.allowPathUpdate = true;
        updateState = true;
    }
    
    public void LateUpdate() {
        // This can be switched to from ANY state.
        if (updateState) {
            // Check if I should Slash Attack.
            if (giantSkel_SlashAtk.CheckToSlash()) {
                 brain.SetActiveState(SlashTarget);
            }
            // Check if I should Ground Attack.
            if (giantSkel_GroundAtk.CheckToAttack()) {
                brain.SetActiveState(GroundAttackTarget);
            }
            // This will run the active state. (Function)
            brain.FSMUpdate();
            // Show what function is being run.
            if (debugs && brain.activeState != null) {
                curState = brain.activeState.Method.ToString();
            }
        }
    }
    // --- ACTIVE STATE FUNCTION (SLASH TARGET) ---
    public void SlashTarget() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("SlashTarget: Initial state setup.");
            eRefs.eFollowPath.StopAllMovementCoroutines();
            giantSkel_SlashAtk.StartSlashAttack();
            stateStarted = true;
            return;
        }
        if (debugs) print("SlashTarget: In state.");
        // -- EXIT CONDITION --
        // If I am no longer doing my Slash Attack.
        if (!giantSkel_SlashAtk.inAtk){
            if (debugs) print("SlashTarget: Exiting state.");
            eRefs.eFollowPath.allowPathUpdate = true;
            brain.SetActiveState(Neutral);
            stateStarted = false;
            return;
        }
    }

    public void GroundAttackTarget() {
        if (!stateStarted) {
            if (debugs) print("GroundAttackTarget: Initial state setup.");
            eRefs.eFollowPath.StopAllMovementCoroutines();
            giantSkel_GroundAtk.StartGroundAttack();
            stateStarted = true;
            return;
        }
        if (debugs) print("GroundAttackTarget: In state.");
        // -- EXIT CONDITION --
        // If I am no longer doing my Ground Attack.
        if (!giantSkel_GroundAtk.inAtk){
            if (debugs) print("GroundAttackTarget: Exiting state.");
            eRefs.eFollowPath.allowPathUpdate = true;
            brain.SetActiveState(Neutral);
            stateStarted = false;
            return;
        }
    }

    // --- ACTIVE STATE FUNCTION (CHASE TARGET) ---
    public void ChaseTarget() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("ChaseTarget: Initial state setup.");
            movement_Chase.AssignChaseTarget();
            eRefs.eFollowPath.TriggerFreePath();
            stateStarted = true;
            return;
        }
        if (debugs) print("ChaseTarget: In state.");
        // -- EXIT CONDITION --
        // If my chase timer runs out, wait.
        if (!movement_Chase.inChase) {
            if (debugs) print("ChaseTarget: Switching state to: Wait");
            brain.SetActiveState(Wait);
            stateStarted = false;
        }
        // Maybe if player is a certain distance away trigger a random walk around before chasing again.
    }
    // --- ACTIVE STATE FUNCTION (WAIT) ---
    public void Wait() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("Wait: Initial state setup.");
            enemy_Wait.Wait();
            stateStarted = true;
            return;
        }
        // -- EXIT CONDITION --
        if (!enemy_Wait.inWait) {
            if (debugs) print("Wait: Switching state to: Neutral");
            brain.SetActiveState(Neutral);
            stateStarted = false;
        }
    }  
    // --- ACTIVE STATE FUNCTION (NEUTRAL) ---
    public void Neutral() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("Neutral: Initial state setup.");
            stateStarted = true;
            return;
        }
        if (debugs) print("Neutral: In state.");
        // -- EXIT CONDITION --
        // Chase the target.
        if (debugs) print("Neutral: Switching state to: ChaseTarget.");
        brain.SetActiveState(ChaseTarget);
        stateStarted = false;
        return;
    }
    public void DistanceToPlayer() {
        
    }
}
