﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Actions : MonoBehaviour
{
    public virtual void StartChecks(){
        print("This is from the BASE class. There is most likely a problem here.");
    }
    public virtual void StopActions() {
        print("This is from the BASE class. There is most likely a problem here.");
    }
}
