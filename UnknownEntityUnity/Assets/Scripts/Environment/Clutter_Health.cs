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
    public SO_ObjectDestructionSpawner clutterDestructionSO;
    public SpriteBouncePool spriteBouncePool;

    [Header("Read Only")]
    public float currentDamage;
    public void ReceiveDamage(float damageTaken, Vector2 hittingColliderPos, Vector2 receivingColliderPos) {
        // Add the damage received to check if you are destroyed.
        currentDamage += damageTaken;
        mainSpriteR.sprite = clutterDestructionSO.crackingSprite;
        if (currentDamage >= clutterHealthSO.totalHealth) {
            StartCoroutine(ClutterDestruction(hittingColliderPos, receivingColliderPos));
        } 
    }

    public IEnumerator ClutterDestruction(Vector2 hittingColliderPos, Vector2 receivingColliderPos) {
        // Turn off hit collider, change the sprite for the destroyed version, animate what needs animated.
        obstacleCollider.enabled = false;
        mainSpriteR.sprite = clutterDestructionSO.crackingSprite;
        shadowObj.SetActive(false);
        yield return new WaitForSeconds(clutterDestructionSO.crackedDur);
        // To apply clutter movement based on an impact severity, normalize the direction vector and multiply it by the severity value.
        Vector2 clutterHitDir = ((Vector2)receivingColliderPos - hittingColliderPos).normalized;
        SpriteBounce spriteBounce;
        for (int i = 0; i < clutterDestructionSO.bouncingSpritesSO.Length; i++) {
            spriteBounce = spriteBouncePool.RequestSpriteBounce();
            spriteBounce.transform.position = (Vector2)this.transform.position + clutterDestructionSO.spawnPositions[i];
            spriteBounce.StartBounce(clutterDestructionSO.bouncingSpritesSO[i], clutterHitDir);
            yield return null;
        }
        mainSpriteR.sprite = null;
        hitColliderObj.SetActive(false);
        print("Clutter destroyed, good job, you did it. You are just the best. Never give up.");
    }
}