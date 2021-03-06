using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpriteAnimObject", menuName = "SO_FXs/SpriteAnimObject", order = 0)]
public class SO_SpriteAnimObject : ScriptableObject
{
    // If it is a single clip, it will play animationClips[0]. Otherwise it will go through all the animation clips one after another.
    // public bool singleAnimation;
    //public List<AnimationClip> animationClips = new List<AnimationClip>();

    public AnimationClip animClip;
    // Consider taking inspiration from the values of the SO_AnimationValues script and inproving on them, like having a class for each set of animation values. Ex Firefly; Firefly comes in Anim plays once, then a looping animation where it flies around x amount of times, then firefly exits plays once.
    // Improve and add upon this as needed. 
}
