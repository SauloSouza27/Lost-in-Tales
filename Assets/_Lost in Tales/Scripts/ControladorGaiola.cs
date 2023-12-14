using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorGaiola : MonoBehaviour
{
    [SerializeField] private GameObject openCage, closedCage, goldenChicken;

    private bool redIsCorrect = false, blueIsCorrect = false, greenIsCorrect = false, purpleIsCorrect = false;

    public void SetTrueIfColorIsCorrect(string pathColor)
    {
        if(pathColor == "Red")
        {
            redIsCorrect = true;
        }
        if (pathColor == "Blue")
        {
            blueIsCorrect = true;
        }
        if (pathColor == "Green")
        {
            greenIsCorrect = true;
        }
        if (pathColor == "Purple")
        {
            purpleIsCorrect = true;
        }
    }

    public void SetFalseIfTotemIsRemoved(string pathColor)
    {
        if (pathColor == "Red")
        {
            redIsCorrect = false;
        }
        if (pathColor == "Blue")
        {
            blueIsCorrect = false;
        }
        if (pathColor == "Green")
        {
            greenIsCorrect = false;
        }
        if (pathColor == "Purple")
        {
            purpleIsCorrect = false;
        }
    }

    private void Update()
    {
        Debug.Log("Red"+redIsCorrect);
        Debug.Log("Blue"+blueIsCorrect);
        Debug.Log("Green"+greenIsCorrect);
        Debug.Log("Purple"+purpleIsCorrect);
    }
}
