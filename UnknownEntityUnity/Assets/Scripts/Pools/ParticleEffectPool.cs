using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPool : MonoBehaviour
{
        public List<ParticleEffectObject> ParticleEffectObjects = new List<ParticleEffectObject>();
    public GameObject poolPrefab;

    public ParticleEffectObject RequestParticleEffect() {
        // Go through the list for a projectile that is not inUse, potentially change this to transfer game object from an availible list to an in use one back and forth, pop from the back. SOmething like that. Or tranfer from and to the same index.
        foreach (ParticleEffectObject effect in ParticleEffectObjects)
        {
            if (!effect.inUse) {
                return effect;
            }
        }
        ParticleEffectObjects.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<ParticleEffectObject>());
        return ParticleEffectObjects[ParticleEffectObjects.Count-1];
    }
}
