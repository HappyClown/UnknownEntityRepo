using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupOrder : MonoBehaviour
{
    public AGrid aGrid;
    public CreateLevelTiles walkerRoomGen;
    public PremadeRoomLevelGeneration premadeRoomLvlGen;

    void Update() {
        if (Input.GetKeyDown("c")) {
            walkerRoomGen.SetupCreateLevel();
            aGrid.SetupCreateGrid();
        }
        if (Input.GetKeyDown("v")) {
            premadeRoomLvlGen.SetupCreateLevel();
            aGrid.SetupCreateGrid();
        }
    }
}
