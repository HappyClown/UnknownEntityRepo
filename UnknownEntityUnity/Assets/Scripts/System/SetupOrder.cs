using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupOrder : MonoBehaviour
{
    public AGrid aGrid;
    public CreateWalkerLevelTiles walkerRoomGen;
    public PremadeRoomLevelGeneration premadeRoomLvlGen;
    public LevelGrid lvlGrid;

    void Update() {
        if (Input.GetKeyDown("c")) {
            walkerRoomGen.SetupCreateLevel();
            aGrid.SetupCreateGrid();
        }
        if (Input.GetKeyDown("v")) {
            premadeRoomLvlGen.SetupCreateLevel();
            aGrid.SetupCreateGrid();
        }
        if (Input.GetKeyDown("b")) {
            StartCoroutine(SetupThree());
        }
    }
    IEnumerator SetupThree() {
        lvlGrid.CreateLevel();
        yield return null;
        aGrid.SetupCreateGrid();
    }
}
