﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AttackFX", menuName = "SOAttackFX", order = 0)]
public class SO_AttackFX : ScriptableObject {
    public float totalDuration;
    [Header("Collision")]
    public PolygonCollider2D collider; // Maybe need more then one in the future.
    public float colStart, colEnd;
    [Header("Sprite Changes")]
    public Sprite[] sprites;
    public float[] spriteChangeTiming; // Should be spread across the totalDuration.
    [Header("Movement")]
    public float spawnDistance;
    public float moveDistance; // Currently moveDistance - spawnDistance for the actual movement.
    public float moveDelay; // Currently unused.
    public AnimationCurve moveAnimCurve;
}