using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeDeFeijao : MonoBehaviour
{
    [SerializeField] private GameObject feijao, peDeFeijao, uiVictory;
    [SerializeField] private float timeForSwitch = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FEIJAO")
        {
            SwitchBeanForBeanTree();
        }
    }

    private void SwitchBeanForBeanTree()
    {
        StartCoroutine(SetActiveFeijao(false));
        StartCoroutine(SetActivePeDeFeijao(true));
        StartCoroutine(SetVictoryMenu());
    }

    private IEnumerator SetActiveFeijao(bool active)
    {
        yield return new WaitForSeconds(timeForSwitch);
        if (active)
        {
            feijao.SetActive(true);
        }
        else
        {
            feijao.SetActive(false);
        }
    }
    private IEnumerator SetActivePeDeFeijao(bool active)
    {
        yield return new WaitForSeconds(timeForSwitch + 0.5f);

        if (active)
        {
            peDeFeijao.SetActive(true);
        }
        else
        {
            feijao.SetActive(false);
        }
    }

    private IEnumerator SetVictoryMenu()
    {
        yield return new WaitForSeconds(2.8f);
        uiVictory.SetActive(true);
        Time.timeScale = 0f;
    }
}
