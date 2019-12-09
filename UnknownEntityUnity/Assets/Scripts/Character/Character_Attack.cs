using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Attack : MonoBehaviour
{
    public MouseInputs moIn;
    public Transform weaponTran;
    private bool weaponFlip = false;
    public float weaponAngle = 90f;
    public Animator weapTrailAnimr;
    public Transform weapTrailTrans;
    public Transform weapOrigTran;
    public SpriteRenderer weapSpriteR;
    public float attackCooldown;
    private float atkCDTimer;
    private bool rotateWeapon;
    public float rotDur;
    private float tarZEuler, iniZEuler, diffZEuler, curZEuler, rotTimer;

    void Update() {
        if (atkCDTimer >= attackCooldown && moIn.mouseLeftClicked) {
            if (weaponFlip) {
                weaponAngle = 120f;
                diffZEuler = -30f;
                weaponFlip = false;
            }
            else {
                weaponAngle = -120f;
                diffZEuler = 30f;
                weaponFlip = true;
            }
            atkCDTimer = 0f;
            weapSpriteR.color= Color.gray;
            weapTrailAnimr.SetTrigger("Slash");
            //weapOrigTran.localScale = new Vector3(weapOrigTran.localScale.x * -1, weapOrigTran.localScale.y, weapOrigTran.localScale.z);
            weapTrailTrans.localScale = new Vector3(weapTrailTrans.localScale.x * -1, weapTrailTrans.localScale.y, weapTrailTrans.localScale.z);
            weaponTran.localEulerAngles = new Vector3(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, weaponTran.localEulerAngles.z + weaponAngle);
            iniZEuler = weaponTran.localEulerAngles.z;
            tarZEuler = weaponTran.localEulerAngles.z + diffZEuler;
            rotateWeapon = true;
            // TEMP to set the trail position to the weapon trail point position.
            weapTrailAnimr.gameObject.transform.parent.position = weapOrigTran.transform.position;
            weapTrailAnimr.gameObject.transform.parent.eulerAngles = weapOrigTran.transform.eulerAngles;
            weapTrailAnimr.gameObject.transform.parent.localScale = weapOrigTran.transform.localScale;
        }
        // After instant move, small lerp rotation.
        if (rotateWeapon) {
            rotTimer += Time.deltaTime / rotDur;
            curZEuler = Mathf.LerpAngle(iniZEuler, tarZEuler, rotTimer);
            if (rotTimer >= 1f) {
                rotateWeapon = false;
                rotTimer = 0f;
                curZEuler = tarZEuler;
            }
            weaponTran.localEulerAngles = new Vector3(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZEuler);
        }
        // Attack cooldown.
        if (atkCDTimer < attackCooldown) {
            atkCDTimer += Time.deltaTime;
            if (atkCDTimer >= attackCooldown) {
                weapSpriteR.color= Color.white;
            }
        }
    }
}
