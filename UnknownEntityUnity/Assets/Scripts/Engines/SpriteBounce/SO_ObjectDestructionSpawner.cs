using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpriteBounce", menuName = "SpriteBounce")]
public class SO_ObjectDestructionSpawner : ScriptableObject
{
    public SO_SpriteBounce[] bouncingSpritesSO; // Scriptable Objects for each piece that will spawn from the object getting destroyed.
    public Vector2[] spawnPositions; // Where the pieces spawn in relation to the object getting destroyed's pivot.
}
