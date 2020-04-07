using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyXYZTransform : MonoBehaviour
{    
    public Transform target;

    void LateUpdate() {
        this.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);    
    }
}
