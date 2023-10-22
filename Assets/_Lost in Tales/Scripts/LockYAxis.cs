using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LockYAxis : MonoBehaviour
{
    private float initialYPosition;

    private PlayerController isMovingReference;

    private Vector3 initialPosition;

    private bool value;

    private void Start()
    {
        initialYPosition = transform.position.y;
        isMovingReference = GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        Vector3 newPosition = new Vector3(transform.position.x, initialYPosition, transform.position.z);
        transform.position = newPosition;

    }

    void OnTriggerEnter(Collider collision) {

        if (collision.CompareTag("static"))
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
            transform.position = initialPosition;
        }

        
    }
}