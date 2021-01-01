using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpriteBounce", menuName = "SpriteBounce")]
public class SO_SpriteBounce : ScriptableObject
{
    [Header("To set per object part")]
    public Sprite sprite;
    public float minMoveDist, maxMoveDist;
    public float moveDist {
        get{ return Random.Range(minMoveDist, maxMoveDist); }
    }
    public float slidePercentOfDist;
    public float minStartSpeed, maxStartSpeed;
    public float startSpeed {
        get{ return Random.Range(minStartSpeed, maxStartSpeed); }
    }
    public int minBounces, maxBounces;
    public int bounces {
        get{ return Random.Range(minBounces, maxBounces+1); }
    }
    
    private float minBounceHeight, maxBounceHeight;
    public float bounceHeight {
        get{ return Random.Range(minBounceHeight, maxBounceHeight); }
    }
    public float heightLossMult;
    public AnimationCurve bounceCurve;
    public float radius;
    public float minRotation, maxRotation;
    public float rotation {
        get{ return Random.Range(minRotation, maxRotation); }
    }
    public ContactFilter2D contactFilter;
}
