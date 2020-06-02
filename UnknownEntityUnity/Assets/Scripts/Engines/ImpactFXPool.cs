using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFXPool : MonoBehaviour
{
    public List<ImpactFX> impactFXs = new List<ImpactFX>();
    public GameObject poolPrefab;

    public ImpactFX RequestImpactFX() {
        // Go through the list for a projectile that is not inUse, potentially change this to transfer game object from an availible list to an in use one back and forth, pop from the back. SOmething like that. Or tranfer from and to the same index.
        foreach (ImpactFX fx in impactFXs)
        {
            if (!fx.inUse) {
                return fx;
            }
        }
        impactFXs.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<ImpactFX>());
        return impactFXs[impactFXs.Count-1];
    }
}