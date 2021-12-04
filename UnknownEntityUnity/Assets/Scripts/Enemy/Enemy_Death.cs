using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Enemy_Refs eRefs;
    // Death sprite anim to be put in a scriptable object or in animation form.
    public Sprite deadSprite;
    public bool isDead = false;
    public GameObject[] objectsToTurnOff;
    public Collider2D myHitCol;
    // Array of SOdestructionobjectspawners each with the limited pieces, randomness applied in this script, set max amount of objects to spawn from each category.
    public SO_ObjectDestructionSpawner[] sODestructionSpawners;
    public SpriteBouncePool spriteBouncePool;
    public int[] amountToSpawn;
    public bool[] random;
    public Enemy_SpecificDeath enemySpecificDeath;

    // Or just destroy the game object now and instatiate a sprite for the death anim. Destroying the game object will need to be done at some point.
    public void DeathSequence(Vector2 hitDir) {
        // Close or stop all active scripts (might be avoided by creating a state machine).
        myHitCol.enabled = false;
        eRefs.eFollowPath.StopAllMovementCoroutines();
        eRefs.eAction.StopActions();
        eRefs.eAction.enabled = false;
        eRefs.eDrops.CheckHealthDrop(this.transform.position, eRefs.charHealth.CurrentHealthPercentage());
        StopEnemyCoroutines();
        // Spawn enemy bits.
        StartCoroutine(EnemyDestruction(hitDir));
        // Turn off everyobject that is not needed by the death animation.
        foreach(GameObject obj in objectsToTurnOff) {
            obj.SetActive(false);
        }
        // Run enemy specific on death methods.
        if (enemySpecificDeath != null) {
            enemySpecificDeath.PlayOnDeath();
        }
        // Play death animation.
        isDead = true;
        // Signal to the level enemy list that an enemy has died.
        eRefs.enemyList.EnemyHasDied();
    }

    void StopEnemyCoroutines () {
        eRefs.eFollowPath.StopAllCoroutines();
        eRefs.eAggro.StopAllCoroutines();
    }

    IEnumerator EnemyDestruction (Vector2 hitDir) {
        float waitTimeOnCracked = 0f;
        foreach (SO_ObjectDestructionSpawner soObjDest in sODestructionSpawners)
        {
            if (soObjDest.crackedDur > waitTimeOnCracked) {
                waitTimeOnCracked = soObjDest.crackedDur;
                eRefs.eSpriteR.sprite = soObjDest.crackingSprite;
                //print(soObjDest.crackingSprite);
            }
        }
        yield return new WaitForSeconds(waitTimeOnCracked);
        SpriteBounce spriteBounce; 
        // Go through multiple destruction bits spawners.
        for (int i = 0; i < sODestructionSpawners.Length; i++) {
            //print("sODestructionSpawner go by now.");
            // Make a results array the size of the the amount of bits to spawn.
            // int[] results = new int[amountToSpawn[i]];
            // int roll;
            // for (int j = amountToSpawn[i]-1; j >= 0; j--) {
            //     roll = Random.Range(0, amountToSpawn[i]+1);
            //     results[j] = roll;
            //     //print("Roolll rol rol your boat "+roll);
            // }
            foreach (int result in ListShuffle(sODestructionSpawners[i].bouncingSpritesSO.Length, amountToSpawn[i])) {
                spriteBounce = spriteBouncePool.RequestSpriteBounce();
                spriteBounce.transform.position = (Vector2)this.transform.position + sODestructionSpawners[i].spawnPositions[result];
                spriteBounce.StartBounce(sODestructionSpawners[i].bouncingSpritesSO[result], hitDir);
                yield return null;
            }
        }
        eRefs.eSpriteR.sprite = null;
        yield return null;
    }
    public List<int> ListShuffle(int amountOfPieces, int amountToSpawnOnce) {
        int randomRoll;
        int amountLeft = amountOfPieces;
        List<int> resultList = new List<int>();
        // Initialize list from 0 to desired length.
        List<int> scratchList = new List<int>(new int[amountOfPieces]);
        for (int i = 0; i < amountOfPieces; i++) {
            scratchList[i] = i;
        }
        //
        //print("New List: ");
        for (int i = 0; i < amountToSpawnOnce; i++) {
            randomRoll = Random.Range(0, amountLeft);
            //print(randomRoll);
            resultList.Add(scratchList[randomRoll]);
            amountLeft--;
            scratchList[randomRoll] = amountLeft;
        }
        // print("New List: ");
        // foreach(int result in resultList) {
        //     print(result);
        // }

        return resultList;
    }
}
