using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimPool : MonoBehaviour
{
    public List<SpriteAnimObject> spriteAnimObjects = new List<SpriteAnimObject>();
    public GameObject poolPrefab;

    public SpriteAnimObject RequestSpriteAnimObject() {
        foreach (SpriteAnimObject animObj in spriteAnimObjects)
        {
            if (!animObj.inUse) {
                return animObj;
            }
        }
        spriteAnimObjects.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<SpriteAnimObject>());
        return spriteAnimObjects[spriteAnimObjects.Count-1];
    }
}
