using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TileRoom", menuName = "SO_LevelCreationStuff/TileRoom")]
public class SO_TileRoomBase : ScriptableObject
{
    [System.Serializable]
    public class RowNumber {
        public int[] amntOfRows;
    }
    public RowNumber[] columns;

    
}
