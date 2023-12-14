using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsColor : MonoBehaviour
{
    [SerializeField] private bool isRed, isGreen, isBlue, isPurple;

    private MeshRenderer meshRenderer;
    private Material material;

    [SerializeField] private ControladorGaiola controladorGaiola;

    private void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.EnableKeyword("_EMISSION");
        SetPathColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "totemRed")
        {
            if (isRed)
            {
                controladorGaiola.SetTrueIfColorIsCorrect("Red");
            }
        }
        if (collision.gameObject.name == "totemGreen")
        {
            if (isGreen)
            {
                controladorGaiola.SetTrueIfColorIsCorrect("Green");
            }
        }
        if (collision.gameObject.name == "totemBlue")
        {
            if (isBlue)
            {
                controladorGaiola.SetTrueIfColorIsCorrect("Blue");
            }
        }
        if (collision.gameObject.name == "totemPurple")
        {
            if (isPurple)
            {
                controladorGaiola.SetTrueIfColorIsCorrect("Purple");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "totemRed")
        {
            if (isRed)
            {
                controladorGaiola.SetFalseIfTotemIsRemoved("Red");
            }
            
        }
        if (collision.gameObject.name == "totemGreen")
        {
            if (isGreen)
            {
                controladorGaiola.SetFalseIfTotemIsRemoved("Green");
            }
        }
        if (collision.gameObject.name == "totemBlue")
        {
            if (isBlue)
            {
                controladorGaiola.SetFalseIfTotemIsRemoved("Blue");
            }
        }
        if (collision.gameObject.name == "totemPurple")
        {
            if (isPurple)
            {
                controladorGaiola.SetFalseIfTotemIsRemoved("Purple");
            }
        }
    }

    private void SetPathColor()
    {
        if (isRed)
        {
            material.color = Color.red;
            material.SetColor("_EmissionColor", Color.red);
        }
        if (isGreen)
        {
            material.color = Color.green;
            material.SetColor("_EmissionColor", Color.green);
        }
        if (isBlue)
        {
            material.color = Color.cyan;
            material.SetColor("_EmissionColor", Color.cyan);
        }
        if (isPurple)
        {
            material.color = Color.magenta;
            material.SetColor("_EmissionColor", Color.magenta);
        }
    }

}
