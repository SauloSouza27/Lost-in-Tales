using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private float initialYPosition;

    private PlayerController isMovingReference;

    private Vector3 initialPosition;

    public bool collisionCheck = false;

    private bool value;

    private Vector3 provisorioPosition;

    public AudioSource audiosource;

    // Visual Feedback
    private MeshRenderer meshRenderer;
    private Material material;
    private Light onBoxLight;

    private float t = 0f;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        provisorioPosition = transform.position;
        initialYPosition = transform.position.y;
        isMovingReference = GetComponent<PlayerController>();
        initialPosition = transform.position;

        // Visual Feedback
        meshRenderer = this.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.DisableKeyword("_EMISSION");
        onBoxLight = this.GetComponentInChildren<Light>();
    }

    private void Update()
    {
        if (onBoxLight != null && onBoxLight.enabled)
        {
            onBoxLight.intensity = Mathf.PingPong(t/3, 0.4f);
            t += Time.deltaTime;
        }

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

        // Visual Feedback
        if (collision.CompareTag("Player"))
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Visual Feedback
        if (other.CompareTag("Player"))
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Visual Feedback
            material.DisableKeyword("_EMISSION");
        }
    }

    public void SetInitialPosition(Vector3 actualPos)
    {
        initialPosition = actualPos;
    }

    public void SetActiveOnBoxLight(bool active)
    {
        t = 0f;
        if (active)
        {
            onBoxLight.enabled = true;
        }
        else
        {
            onBoxLight.enabled = false;
        }
    }
}
