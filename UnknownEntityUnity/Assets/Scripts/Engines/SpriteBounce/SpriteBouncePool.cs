using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBouncePool : MonoBehaviour
{
   public List<SpriteBounce> spriteBounce = new List<SpriteBounce>();
    public GameObject poolPrefab;

    public SpriteBounce RequestSpriteBounce() {
        // Go through the list for a projectile that is not inUse, potentially change this to transfer game object from an availible list to an in use one back and forth, pop from the back. SOmething like that. Or tranfer from and to the same index.
        foreach (SpriteBounce sprite in spriteBounce)
        {
            if (!sprite.inUse) {
                return sprite;
            }
        }
        spriteBounce.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<SpriteBounce>());
        return spriteBounce[spriteBounce.Count-1];
    }
}
