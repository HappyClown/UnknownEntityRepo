using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton_Actions : Enemy_Actions
{
    [Header ("Script References")]
    public ShieldSkeleton_ShieldBash shieldBash;
    public ShieldSkeleton_ShieldUp shieldUp;
    public Enemy_Movement_Defend movement_Defend;
    public Enemy_Movement_Chase movement_Chase;
    public Enemy_Defender eDefender;
    public Enemy_Refs eRefs;
    [Header ("To-set Variables")]
    public bool debugs;
    public float chaseDistance;
    public float closeToDefensePoint;
    public float farFromDefensePoint;

    [Header ("Read Only")]
    public string curState;
    public bool isShieldUp;
    public float ChaseDistanceSqr {
        get {
            return chaseDistance * chaseDistance;
            }
    }
    public float CloseToDefensePointSqr {
        get {
            return closeToDefensePoint * closeToDefensePoint;
            }
    }
    public float FarFromDefensePointSqr {
        get {
            return farFromDefensePoint * farFromDefensePoint;
            }
    }
    bool stateStarted = false;

    public override void StartChecks() {
        // Create a new state machine for this enemy. 
        // The "checkingAggro" state should be implemented in the state machine only if needed.
        brain = new FSM();
        // Set initial state.
        brain.SetActiveState(DefendAlly);
    }

    public override void StopActions() {
        shieldBash.StopAction();
    }
    

    // --- ACTIVE STATE FUNCTION (DEFEND ALLY) ---
    public void DefendAlly() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("DefendAlly: Initial state setup.");
            if (movement_Defend.CheckAlly()) {
                movement_Defend.AssignTokenToPathTarget();
                movement_Defend.StartDefendingAlly();
            }
            eRefs.eFollowPath.TriggerFreePath();
            shieldUp.AllowShieldUp();
            stateStarted = true;
            return;
        }
        if (debugs) print("DefendAlly: In state.");
        // -- EXIT CONDITIONS --
        // If the allyToDefend is dead.
        if (!eDefender.canDefend) {
            if (debugs) print("DefendAlly: Switching state to: ChaseTargetNoShield.");
            brain.SetActiveState(ChaseTargetNoShield);
            stateStarted = false;
            return;
        }
        // If the player is closer to the allyToDefend, start chasing the player, shield down.
        if (!PlayerFartherFromAlly()){
            if (debugs) print("DefendAlly: Switching state to: ChaseTargetNoShield.");
            brain.SetActiveState(ChaseTargetNoShield);
            stateStarted = false;
            return;
        }
        // If the player is within range and im close to my defense point, start chasing it.
        if (PlayerWithinChaseRange() && CloseEnoughToDefensePoint()){
            if (debugs) print("DefendAlly: Switching state to: ChaseTarget.");
            brain.SetActiveState(ChaseTarget);
            stateStarted = false;
            return;
        }
    }

    // --- ACTIVE STATE FUNCTION (CHASE TARGET NO SHIELD) ---
    public void ChaseTargetNoShield() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("ChaseTargetNoShield: Initial state setup.");
            movement_Chase.AssignChaseTarget();
            shieldUp.ForceShieldDown();
            if (!eDefender.canDefend && shieldUp.forceShieldDown) {
                shieldUp.AllowShieldUp();
            }
            eRefs.eFollowPath.TriggerFreePath();
            stateStarted = true;
            return;
        }
        if (debugs) print("ChaseTargetNoShield: In state.");
        // -- EXIT CONDITION --
        // If the player is farther from the allyToDefend then this enemy is, defend the allyToDefend.
        if (eDefender.canDefend && PlayerFartherFromAlly()){
            if (debugs) print("ChaseTargetNoShield: Switching state to: DefendAlly.");
            brain.SetActiveState(DefendAlly);
            stateStarted = false;
            return;
        }
        // If the ally is dead switch to chasing target and allowing shield up.
        if (!eDefender.canDefend){
            if (debugs) print("ChaseTargetNoShield: Switching state to: ChaseTarget.");
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
            movement_Chase.AssignChaseTarget();
            eRefs.eFollowPath.TriggerFreePath();
            shieldUp.AllowShieldUp();
            stateStarted = true;
            return;
        }
        if (debugs) print("ChaseTarget: In state.");
        // -- EXIT CONDITION --
        // If I am too far from the defense point, go back to defend.
        if (eDefender.canDefend && TooFarFromDefensePoint()){
            if (debugs) print("ChaseTarget: Switching state to: DefendAlly.");
            brain.SetActiveState(DefendAlly);
            stateStarted = false;
            return;
        }
        // If player is closer to ally, chase player without shield.
        if (eDefender.canDefend && !PlayerFartherFromAlly()){
            if (debugs) print("ChaseTarget: Switching state to: ChaseTargetNoShield.");
            brain.SetActiveState(ChaseTargetNoShield);
            stateStarted = false;
            return;
        }
    }
    
    // --- ACTIVE STATE FUNCTION (BASH TARGET) ---
    public void BashTarget() {
        // -- ENTER STATE --
        if (!stateStarted) {
            if (debugs) print("BashTarget: Initial state setup.");
            eRefs.eFollowPath.StopAllMovementCoroutines();
            shieldUp.AllowShieldUp();
            shieldBash.StartShieldBash();
            stateStarted = true;
            return;
        }
        if (debugs) print("BashTarget: In state.");
        // -- EXIT CONDITION --
        // If the player is no longer bashing.
        if (!shieldBash.inBash){
            if (debugs) print("BashTarget: Exiting state.");
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
        // If the player is farther from the allyToDefend then this enemy is, defend the allyToDefend.
        if (eDefender.canDefend && PlayerFartherFromAlly()){
            if (debugs) print("Neutral: Switching state to: DefendAlly.");
            brain.SetActiveState(DefendAlly);
            stateStarted = false;
            return;
        }
        // If the player is closer to the allyToDefend, start chasing the player, shield down.
        if (eDefender.canDefend && !PlayerFartherFromAlly()){
            if (debugs) print("Neutral: Switching state to: ChaseTargetNoShield.");
            brain.SetActiveState(ChaseTargetNoShield);
            stateStarted = false;
            return;
        }
        // If I there is nothing to defend, just chase the target.
        if (!eDefender.canDefend){
            if (debugs) print("Neutral: Switching state to: ChaseTarget.");
            brain.SetActiveState(ChaseTarget);
            stateStarted = false;
            return;
        }
    }
    
    
    public bool PlayerFartherFromAlly() {
        if (eRefs.SqrDistToTarget(movement_Defend.allyTrans.position, eRefs.PlayerPos) > eRefs.SqrDistToTarget(movement_Defend.allyTrans.position, this.transform.position)) {
            //if (debugs) print("Player is farther them me from ally, can chill.");
            return true;
        }
        return false;
    }
    public bool PlayerWithinChaseRange() {
        if (eRefs.SqrDistToTarget(this.transform.position, eRefs.PlayerPos) < ChaseDistanceSqr) {
            //if (debugs) print("Player is within chase range, letsa goooooo!");
            return true;
        }
        return false;
    }
    public bool CloseEnoughToDefensePoint() {
        if (eRefs.SqrDistToTarget(this.transform.position, movement_Defend.defensePosTokenTrans.position) < CloseToDefensePointSqr) {
            //if (debugs) print("Enemy is close enough to defense position, it may go.");
            return true;
        }
        return false;
    }
    public bool TooFarFromDefensePoint() {
        if (eRefs.SqrDistToTarget(this.transform.position, movement_Defend.defensePosTokenTrans.position) > FarFromDefensePointSqr) {
            //if (debugs) print("Enemy to far from defense position, go back, go back!");
            return true;
        }
        return false;
    }
    
    public void Update() {
        // This can be switched to from ANY state.
        // If the player is within attack range.
        if (shieldBash.CheckToBash() && brain.activeState != BashTarget) {
            if (debugs) print("ANYSTATE: Switching state to: BashTarget.");
            //brain.prevState = brain.activeState;
            brain.SetActiveState(BashTarget);
            stateStarted = false;
            return;
        }
        // This will run the active state. (Function)
        brain.FSMUpdate();
        // Show what function is being run.
        if (debugs && brain.activeState != null) {
            curState = brain.activeState.Method.ToString();
            isShieldUp = shieldUp.shieldIsUp;
        }
    }
}
