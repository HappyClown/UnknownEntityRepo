using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpritePool : MonoBehaviour
{
    public List<StaticSpritePoolObject> staticSpritePoolObject = new List<StaticSpritePoolObject>();
    public GameObject poolPrefab;

    public StaticSpritePoolObject RequestStaticSpritePoolObject() {
        foreach (StaticSpritePoolObject statObj in staticSpritePoolObject)
        {
            if (!statObj.inUse) {
                return statObj;
            }
        }
        staticSpritePoolObject.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<StaticSpritePoolObject>());
        return staticSpritePoolObject[staticSpritePoolObject.Count-1];
    }
}
