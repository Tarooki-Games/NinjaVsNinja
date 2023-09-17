using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 1f;
    void Update()
    {
        transform.Rotate(0, 0, -_rotationSpeed);
    }
}