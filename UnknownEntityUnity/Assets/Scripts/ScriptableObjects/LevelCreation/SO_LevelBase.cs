using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class SO_LevelBase : ScriptableObject
{
    public int minRoomAmnt, maxRoomAmnt;
    public int minRoomSize, maxRoomSize;
    public List<SO_RoomBase> rooms = new List<SO_RoomBase>();
    public int difficulty;
}