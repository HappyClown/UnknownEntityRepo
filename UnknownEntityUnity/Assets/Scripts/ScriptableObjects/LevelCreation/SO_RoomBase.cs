using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class SO_RoomBase : ScriptableObject
{
    public GameObject roomPrefab;
    public PremadeRoom prefabRoomScript;
}
