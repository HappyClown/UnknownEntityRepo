using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character_MovementSkills : MonoBehaviour
{
    // public MouseInputs moIn;
    // public Character_Movement charMove;
    // public Character_MovementSkills chosenMoveSkill;

    // void Update() {
    //     if (moIn.interactPressed) {
    //         chosenMoveSkill.StartMovementSkill();
    //     }
    // }

    //public virtual void StartMovementSkill() {}
    public abstract bool CanIUseMovementSkill();
}
