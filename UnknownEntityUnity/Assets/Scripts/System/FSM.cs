using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public delegate void Del();
    public Del activeState;

    public void SetActiveState(Del newState) {
        activeState = newState;
    }

    public void FSMUpdate() {
        if (activeState != null) {
            activeState();
        }
    }
}
