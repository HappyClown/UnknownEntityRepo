using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class SO_RoomBase : ScriptableObject
{
    public Vector3[] spawnPoints;
    public Vector3[] roomConnections;

}
