using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Clutter_ObjectGroup", menuName = "SO_Environment/Clutter_ObjectGroup")]
public class SO_Clutter_ObjectGroup : ScriptableObject
{
    public GameObject groupGO;
    public int pointCost;
    [Header("Physical Attributes")]
    public Vector2 offsetInTile;
    
}