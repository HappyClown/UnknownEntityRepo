using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AnimValues", menuName = "AnimValues")]
public class SO_AnimationValues : ScriptableObject
{
    [Header("Looping Values")]
    public bool loop;
    public int loopAmnt;
    public bool spriteOnCooldown;
    public Sprite cooldownSprite;
    public float minCooldown, maxCooldown;
    public float Cooldown {
        get {
            return Random.Range(minCooldown, maxCooldown);
        }
    }
    [Header("Animation Values")]
    public Color color = Color.white;
    public float totalDuration;
    public Sprite[] sprites;
    public float[] spriteTimings;
    public UnityEvent[] unityEvents;
    public string[] eventMessages;
    public float[] eventTriggers;
}