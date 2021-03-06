using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoomLayouts", menuName = "SO_LevelCreationStuff/RoomLayouts")]
public class SO_RoomLayouts : ScriptableObject
{
    public SO_TileRoomBase[] TilesXByTilesY;
}
