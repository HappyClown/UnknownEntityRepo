using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSequence : MonoBehaviour
{
    [Header("Objects")]
    public GameObject backGround;
    public SpriteRenderer backGroundSR;
    public Sprite backGroundS;
    //
    public GameObject titleText;
    public SpriteRenderer titleTextSR;
    public Sprite titleTextS;
    //
    public GameObject restart;
    public SpriteRenderer restartSR;
    public Sprite restartS;
    [Header("To-Set Variables")]
    public Transform playerTrans;
    public SpriteRenderer playerSR;
    public MouseInputs moIn;
    public CameraFollow camFollow;
    public CameraDeath camDeath;
    public float backGroundAppearDelay;
    public float titleTextAppearDelay;
    public float restartAppearDelay;
    [Header("Ready Only")]
    public bool restartAvailable;

    void Update() {
        if (moIn.confirmPressed && restartAvailable) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }

    public IEnumerator DeathUI() {
        this.transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y, this.transform.position.z);
        camFollow.enabled = false;
        camDeath.DeathCamLerpSetup();
        yield return new WaitForSeconds(backGroundAppearDelay);
        playerSR.sortingLayerName = "UI";
        playerSR.sortingOrder = 1;
        backGround.SetActive(true);
        yield return new WaitForSeconds(titleTextAppearDelay);
        titleText.SetActive(true);
        yield return new WaitForSeconds(restartAppearDelay);
        restart.SetActive(true);
        restartAvailable = true;
        yield return null;
    }
}
