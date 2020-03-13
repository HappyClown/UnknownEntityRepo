using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool inUse;
    float timer;
    SO_Projectile projSO;
    public SpriteRenderer mySpriteR;
    public Collider2D myCol;
    Sprite[] animSprites;
    //ContactFilter2D contactFilter;
    List<Collider2D> collidedList = new List<Collider2D>(1);
    //Vector2 direction;

    public void LaunchProjectile(SO_Projectile _projSO, Vector2 _direction, Vector2 startPos) {
        inUse = true;
        timer = 0f;
        projSO = _projSO;
        //contactFilter = projSO.contactFilter;
        //direction = _direction;
        this.transform.position = new Vector3(startPos.x, startPos.y, this.transform.position.z);
        this.transform.up = _direction;
        // Check if the sprite is animated, if yes start animation coroutine? Else just take the single sprite.
        if (!projSO.animated) {
            mySpriteR.sprite = projSO.sprite;
        }
        else {
            // Setup animation.
            animSprites = projSO.animSprites;
        }
        // Assign collider.
        //myCol = projSO.col;
        this.gameObject.SetActive(true);
        StartCoroutine(MoveProjectile());
    }

    IEnumerator MoveProjectile() {
        while (timer < projSO.duration) {
            timer += Time.deltaTime;
            // Move the projectile.
            transform.Translate(Vector2.up * projSO.speed * Time.deltaTime);
            // Check collisions.
            if (myCol.OverlapCollider(projSO.contactFilter, collidedList) > 0) {
                print("Should check colliders");
                CheckColliders();
            }
            yield return null;
        }
        inUse = false;
        this.gameObject.SetActive(false);
        yield return null;
    }

    void CheckColliders() {
        if (collidedList.Count > 0 && collidedList[0] != null) {
            foreach(Collider2D col in collidedList) {
                if (col.CompareTag("Player")) {
                    // Apply damage to the player.
                    col.GetComponent<Character_Health>().TakeDamage(projSO.Damage);
                }
                else if (col.CompareTag("Destructible")) {
                    // Get destructible script and apply damage to the object.
                }
                else {
                    // This should be the obstacles that just destroy the projectile nothing else.
                }
                // Turn off the projectile with collision.
                inUse = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
