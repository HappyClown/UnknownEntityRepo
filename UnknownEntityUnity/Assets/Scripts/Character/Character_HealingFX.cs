using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_HealingFX : MonoBehaviour
{
    [Header("Character")]
    public SpriteRenderer spriteR;
    public Color playerColorTint;
    public float colorSwitchTime;
    [Header("Spawning")]
    public Transform playerParent;
    public StaticSpritePool staticSpritePool;
    private StaticSpritePoolObject staticSpritePoolObject;
    public Transform spawnTrans;
    public int amountToSpawn;
    public float spawnRate;
    public float totSpawnTime;
    public float spawnCrossTime;
    public float spawnCircleRadius = 1f;
    public GameObject healingCrossFX;
    public bool testStartButton;
    [Header("FXs")]
    public float moveDist;
    public float timeToMove;
    public AnimationCurve fXMoveAnimCurve;
    public Sprite crossFXSprite;
    private Vector2 spawnPos;
    private GameObject lastFXSpawned;


    void Update() {
        if (testStartButton) {
            StartHealingFXOnPlayer();
            testStartButton = false;
        }
    }

    public void StartHealingFXOnPlayer() {
        spriteR.material.SetColor("_Color",playerColorTint);
        StartCoroutine(SpawningHealingFXs());
        StartCoroutine(ColorFlashing());
    }

    IEnumerator SpawningHealingFXs() {
        float timer = 0f;
        float fxTimer = spawnCrossTime;
        while (timer < totSpawnTime) {
            timer += Time.deltaTime;
            fxTimer += Time.deltaTime;
            if (fxTimer >= spawnCrossTime) {
                // Spawn a cross;
                SpawnOneHealingFX();
                fxTimer = 0f;
            }
            yield return null;
        }
        timer = 0f;
        while(timer < timeToMove) {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ColorFlashing() {
        float timer = 0f;
        float switchTimer = 0f;
        float totTime = totSpawnTime + spawnCrossTime;
        while (timer < totTime) {
            timer += Time.deltaTime;
            switchTimer += Time.deltaTime;
            if (switchTimer > colorSwitchTime) {
                ColorSwitch();
                switchTimer = 0f;
            }
            yield return null;
        }
        spriteR.material.SetColor("_Color",Color.white);
    }

    public void ColorSwitch() {
        if (spriteR.material.color == Color.white) {
            spriteR.material.SetColor("_Color",playerColorTint);
        }
        else {
            spriteR.material.SetColor("_Color",Color.white);
        }
    }

    public void SpawnOneHealingFX() {
        spawnPos = (Vector2)spawnTrans.position + (Random.insideUnitCircle*spawnCircleRadius);
        staticSpritePoolObject = staticSpritePool.RequestStaticSpritePoolObject();
        staticSpritePoolObject.transform.position = spawnPos;
        staticSpritePoolObject.transform.parent = playerParent;
        staticSpritePoolObject.spriteR.sprite = crossFXSprite;
        staticSpritePoolObject.StaticSpriteInUse();
        //lastFXSpawned = Instantiate(healingCrossFX, spawnPos, Quaternion.identity, playerParent);
        // Start movement coroutine on that FX object.
        StartCoroutine(FXMovement(staticSpritePoolObject));
    }

    IEnumerator FXMovement(StaticSpritePoolObject _statSpriteObject) {
        StaticSpritePoolObject statSpriteObject = _statSpriteObject;
        float timer = 0f;
        float timeMult = 1/timeToMove;
        Vector2 startPos = statSpriteObject.transform.localPosition;
        Vector2 endPos = startPos+(Vector2.up*moveDist);
        while(timer < 1f) {
            timer += Time.deltaTime*timeMult;
            statSpriteObject.transform.localPosition = Vector2.Lerp(startPos, endPos, fXMoveAnimCurve.Evaluate(timer));
            yield return null;
        }
        statSpriteObject.inUse = false;
        statSpriteObject.gameObject.SetActive(false);
        statSpriteObject.transform.parent = staticSpritePool.transform;
    }
}
