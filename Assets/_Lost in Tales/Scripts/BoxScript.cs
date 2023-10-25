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

    private Vector3 provisorioPosition;

    public AudioSource audiosource;

    

    private void Start()
    {
        provisorioPosition = transform.position;
        initialYPosition = transform.position.y;
        isMovingReference = GetComponent<PlayerController>();
        initialPosition = transform.position;
        
    }

    private void Update()
    {
        if (transform.position != provisorioPosition)
        {

          if (!audiosource.isPlaying)
            {
                audiosource.Play();
                
            }

            provisorioPosition = transform.position;

        }
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
