using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime / 0.01f, Space.Self);
    }
}