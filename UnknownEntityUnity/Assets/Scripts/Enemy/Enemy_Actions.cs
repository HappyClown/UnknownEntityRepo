using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS THE VIRTUAL VERSION OF THE ACTION CLASS.
// public class Enemy_Actions : MonoBehaviour
// {
//     public bool defended;
//     public FSM brain;
//     public virtual void StartChecks(){
//         print("This is from the BASE class. There is most likely a problem here.");
//     }
//     public virtual void StopActions() {
//         print("This is from the BASE class. There is most likely a problem here.");
//     }
//     public virtual void CustomUpdate() {
//         print("This is from the BASE class. There is most likely a problem here.");
//     }
// }

// THIS IS THE ABSTRACT VERSION OF THE ACTION CLASS
// Every abstract method also needs to be declared (override) in the derived classes.
public abstract class Enemy_Actions : MonoBehaviour
{
    public bool defended;
    public FSM brain;
    public abstract void StartChecks();
    public abstract void StopActions();
    public abstract void StopStateUpdates();
    public abstract void ResumeStateUpdates();
    //public abstract void CustomUpdate();
}
