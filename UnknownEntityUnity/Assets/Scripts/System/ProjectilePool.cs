using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public List<Projectile> projectiles;

    public Projectile RequestProjectile() {
        // Go through the list for a projectile that is not inUse, potentially change this to transfer game object from an availible list to an in use one back and forth, pop from the back. SOmething like that. Or tranfer from and to the same index.
        foreach (Projectile proj in projectiles)
        {
            if (!proj.inUse) {
                return proj;
            }
        }
        Debug.LogError("There are no availible projectiles, insert code to create a new one... ^_^'.");
        return null; 
    }
}
