using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float initialYPosition;

    private PlayerController isMovingReference;

    private Vector3 initialPosition;

    public bool collisionCheck = false;

    private bool value;

    private void Start()
    {
        initialYPosition = transform.position.y;
        isMovingReference = GetComponent<PlayerController>();
        initialPosition = transform.position;
        
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
            collisionCheck = true;
            transform.position = initialPosition;
        }
    }

    public void SetInitialPosition(Vector3 actualPos)
    {
        initialPosition = actualPos;
    }
}
