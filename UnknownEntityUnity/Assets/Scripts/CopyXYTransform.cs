using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyXYTransform : MonoBehaviour
{
    public Transform target;

    void LateUpdate() {
        this.transform.position = new Vector3(target.position.x, target.position.y, this.transform.position.z);    
    }
}
