using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pagina : MonoBehaviour
{
    [SerializeField] private GameObject uiPagina;
    private bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isColliding == false)
        {
            StartCoroutine(Page());
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }

    IEnumerator Page()
    {
        yield return new WaitForSeconds(1.0f);
        uiPagina.SetActive(true);
        Time.timeScale = 0f;
    }
    public void PageResume()
    {
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        uiPagina.SetActive(false);
    }
}
