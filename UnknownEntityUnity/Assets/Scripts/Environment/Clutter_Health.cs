using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutter_Health : MonoBehaviour
{
    public SpriteRenderer mainSpriteR;
    public GameObject hitColliderObj;
    public GameObject shadowObj;
    public Collider2D obstacleCollider;
    public SO_Clutter_Health clutterHealthSO;
    public SO_ObjectDestructionSpawner barrelDestructionSO;
    public SpriteBouncePool spriteBouncePool;

    [Header("Read Only")]
    public float currentDamage;
    public void ReceiveDamage(float damageTaken) {
        // Add the damage received to check if you are destroyed.
        currentDamage += damageTaken;
        if (currentDamage >= clutterHealthSO.totalHealth) {
            StartCoroutine(ClutterDestruction());
        } 
    }

    public IEnumerator ClutterDestruction() {
        // Turn off hit collider, change the sprite for the destroyed version, animate what needs animated.
        obstacleCollider.enabled = false;
        mainSpriteR.sprite = barrelDestructionSO.crackingSprite;
        shadowObj.SetActive(false);
        yield return new WaitForSeconds(barrelDestructionSO.crackedDur);
        SpriteBounce spriteBounce;
        for (int i = 0; i < barrelDestructionSO.bouncingSpritesSO.Length; i++) {
            spriteBounce = spriteBouncePool.RequestSpriteBounce();
            spriteBounce.transform.position = (Vector2)this.transform.position + barrelDestructionSO.spawnPositions[i];
            spriteBounce.StartBounce(barrelDestructionSO.bouncingSpritesSO[i]);
            yield return null;
        }
        mainSpriteR.sprite = null;
        hitColliderObj.SetActive(false);
        print("Clutter destroyed, good job, you did it. You are just the best. Never give up.");
    }
}