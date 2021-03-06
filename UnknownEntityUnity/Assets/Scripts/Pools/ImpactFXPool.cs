using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFXPool : MonoBehaviour
{
    public List<ImpactFX> impactFXs = new List<ImpactFX>();
    public GameObject poolPrefab;

    public ImpactFX RequestImpactFX() {
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