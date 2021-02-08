using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class RangedSkeleton_Actions : Enemy_Actions
{
    [Header ("Script References")]
    public RangedSkeleton_ThrowProjectile throwProj;
    public Enemy_Movement_RunAway enemy_RunAway;
    public Enemy_Movement_Chase enemy_Chase;
    public Enemy_Refs eRefs;

    [Header ("To-set Variables")]
    public bool debugs;
    public float minRunAwayDist;

    [Header ("Read Only")]
    public string curState;
    public bool updateState = true;
    float minRunAwayDistSqr;
    bool stateStarted = false;

    public override void StartChecks() {
        // Create a new state machine for this enemy. 
        // The "checkingAggro" state should be implemented in the state machine only if needed.
        brain = new FSM();
        // Set initial state.
        brain.SetActiveState(ChaseTarget);
        // Set squared variables.
        minRunAwayDistSqr = minRunAwayDist * minRunAwayDist;
    }

    public override void StopActions() {
        // Stop the bow attack happening. Starting an attack cooldown and not allowing path update.
        throwProj.ProjectileAttackDone(false);
        throwProj.rsBow.StopAndReset();
        throwProj.rsBow.bowSpriteR.sprite = null;
    }
    public override void StopStateUpdates () {
        brain.SetActiveState(Neutral);
        updateState = false;
    }
    public override void ResumeStateUpdates () {
        eRefs.eFollowPath.allowPathUpdate = true;
        updateState = true;
        brain.SetActiveState(Neutral);
        // Not really state related but fits here the best.
        throwProj.rsBow.bowSpriteR.sprite = throwProj.rsBow.defaultBowSprite;
    }

    // --- ACTIVE STATE FUNCTION (RUN AWAY) ---
    public void RunAway() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("RunAway: Initial state setup.");
            enemy_RunAway.SetupRunAwayPos();
            stateStarted = true;
            return;
        }
        if (debugs) print("RunAway: In state.");
        // -- EXIT CONDITION --
        // If my projectile attack is ready start chasing the player.
        if (throwProj.throwProjReady){
            if (debugs) print("RunAway: Switching state to: ChaseTarget");
            brain.SetActiveState(ChaseTarget);
            stateStarted = false;
            return;
        }
    }    
    // --- ACTIVE STATE FUNCTION (CHASE TARGET) ---
    public void ChaseTarget() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("ChaseTarget: Initial state setup.");
            enemy_Chase.AssignChaseTarget();
            stateStarted = true;
            return;
        }
        if (debugs) print("ChaseTarget: In state.");
        // -- EXIT CONDITION --
        // If the player is too close.
        if (eRefs.SqrDistToTarget(this.transform.position, eRefs.PlayerShadowPos) < minRunAwayDistSqr){
            if (debugs) print("ChaseTarget: Switching state to: RunAway");
            brain.SetActiveState(RunAway);
            stateStarted = false;
            return;
        }
    }

    // --- ACTIVE STATE FUNCTION (THROW PROJECTILE) ---
    public void ThrowProjectile() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("ThrowProjectile: Initial state setup.");
            eRefs.eFollowPath.StopAllMovementCoroutines();
            throwProj.StartProjectileThrow();
            stateStarted = true;
            return;
        }
        if (debugs) print("ThrowProjectile: In state.");
        // -- EXIT CONDITION --
        // If the player is no longer bashing.
        if (!throwProj.inProjThrow){
            if (debugs) print("ThrowProjectile: Exiting state.");
            brain.SetActiveState(Neutral);
            stateStarted = false;
            return;
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
        // If my projectile attack is ready start chasing the player.
        if (throwProj.throwProjReady){
            if (debugs) print("Neutral: Switching state to: ChaseTarget.");
            brain.SetActiveState(ChaseTarget);
            stateStarted = false;
            return;
        }
        // If the player is too close.
        if (!throwProj.throwProjReady){
            if (debugs) print("Neutral: Switching state to: RunAway.");
            brain.SetActiveState(RunAway);
            stateStarted = false;
            return;
        }
    }
    
    public void LateUpdate() {
        // This can be switched to from ANY state.
        if (updateState) {
            // If the player is within attack range. Make this check occur less then once per frame.
            if (throwProj.CheckThrowProj() && brain.activeState != ThrowProjectile) {
                if (debugs) print("ANYSTATE: Switching state to: ThrowProjectile.");
                brain.SetActiveState(ThrowProjectile);
                stateStarted = false;
                return;
            }
            // This will run the active state. (Function)
            brain.FSMUpdate();
            if (debugs && brain.activeState != null) {
                curState = brain.activeState.Method.ToString();
            }
        }
    }
}
