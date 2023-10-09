using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockYAxis : MonoBehaviour
{
    private float initialYPosition;

    private void Start()
    {
        initialYPosition = transform.position.y;
    }

    private void Update()
    {
        Vector3 newPosition = new Vector3(transform.position.x, initialYPosition, transform.position.z);
        transform.position = newPosition;
    }
}