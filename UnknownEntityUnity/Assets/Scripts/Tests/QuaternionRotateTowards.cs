using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionRotateTowards : MonoBehaviour
{
    public float rotationPerSecond, goalAngle;

    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, goalAngle), rotationPerSecond * Time.deltaTime);
    }
}
