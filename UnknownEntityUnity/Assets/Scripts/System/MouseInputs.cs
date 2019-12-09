using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder(-50)]
public class MouseInputs : MonoBehaviour
{
    public Camera cam;
    public Vector3 mousePos;
    public Vector3 mousePosWorld;
    public Vector2 mousePosWorld2D, lastMousePosWorld2D;
    public bool mouseLeftClicked, mouseMoved;

    void Start()
    {
        if (!cam) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    void Update()
    {
        lastMousePosWorld2D = mousePosWorld2D;
        mouseLeftClicked = false;
        if (Input.GetMouseButtonDown(0)) {
            mouseLeftClicked = true;
        }

        mousePos = Input.mousePosition;
        mousePosWorld = cam.ScreenToWorldPoint(mousePos);
        mousePosWorld2D = new Vector2(mousePosWorld.x, mousePosWorld.y);
        if (mousePosWorld2D != lastMousePosWorld2D) {
            mouseMoved = true;
        }
        else {
            mouseMoved = false;
        }
    }
}
