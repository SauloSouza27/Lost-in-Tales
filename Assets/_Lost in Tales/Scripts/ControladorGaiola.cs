using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorGaiola : MonoBehaviour
{
    [SerializeField] private GameObject openCage, closedCage, goldenChicken, uiVictory;

    [SerializeField] private GameObject[] totems;

    private bool redIsCorrect = false, blueIsCorrect = false, greenIsCorrect = false, purpleIsCorrect = false, cageIsClosed = true;

    private void Update()
    {
        if (redIsCorrect && blueIsCorrect && greenIsCorrect && purpleIsCorrect)
        {
            if (cageIsClosed)
            {
                OpenCage();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(SetVictoryMenu());
        Debug.Log("HA");
    }

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

    private void OpenCage()
    {
        closedCage.SetActive(false);
        openCage.SetActive(true);

        foreach (GameObject totem in totems)
        {
            totem.GetComponent<BoxCollider>().enabled = false;
        }

        goldenChicken.GetComponent<BoxCollider>().enabled = true;

        cageIsClosed = false;
    }

    private IEnumerator SetVictoryMenu()
    {
        yield return new WaitForSeconds(0.8f);
        uiVictory.SetActive(true);
        Time.timeScale = 0f;
    } 
}
