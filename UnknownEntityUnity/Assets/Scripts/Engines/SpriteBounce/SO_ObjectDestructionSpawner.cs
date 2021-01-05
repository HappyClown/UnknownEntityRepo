using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpriteBounceParent", menuName = "SpriteBounceParent")]
public class SO_ObjectDestructionSpawner : ScriptableObject
{
    public Sprite crackingSprite; // For example for a barrel, barrel sprite with cracks everywhere on it.
    public float crackedDur; // The duration the cracked sprite will stay on before it 'explodes'.
    public Sprite destroyedSprite; // Barrel example, just a small part of the bottom of the barrel is left at the barrel's location. 
    public SO_SpriteBounce[] bouncingSpritesSO; // Scriptable Objects for each piece that will spawn from the object getting destroyed.
    public Vector2[] spawnPositions; // Where the pieces spawn in relation to the object getting destroyed's pivot.
}
