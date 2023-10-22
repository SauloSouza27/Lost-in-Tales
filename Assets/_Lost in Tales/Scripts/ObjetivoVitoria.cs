using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetivoVitoria : MonoBehaviour
{
    [SerializeField] private GameObject uiVictory;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SetVictoryMenu());
    }

    IEnumerator SetVictoryMenu()
    {
        yield return new WaitForSeconds(1.0f);
        uiVictory.SetActive(true);
        Time.timeScale = 0f;
    }
}
