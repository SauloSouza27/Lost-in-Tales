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

    // Emission Visual Feedback
    private MeshRenderer meshRenderer;
    private Material material;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        provisorioPosition = transform.position;
        initialYPosition = transform.position.y;
        isMovingReference = GetComponent<PlayerController>();
        initialPosition = transform.position;

        // Emission Visual Feedback
        meshRenderer = this.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.DisableKeyword("_EMISSION");
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

        // Emission Visual Feedback
        if (collision.CompareTag("Player"))
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Emission Visual Feedback
        if (other.CompareTag("Player"))
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Emission Visual Feedback
            material.DisableKeyword("_EMISSION");
        }
    }

    public void SetInitialPosition(Vector3 actualPos)
    {
        initialPosition = actualPos;
    }
}
