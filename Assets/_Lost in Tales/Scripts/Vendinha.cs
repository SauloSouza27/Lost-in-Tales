using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendinha : MonoBehaviour
{
    [SerializeField] private GameObject vaca, feijao;
    [SerializeField] private Light luzFeijao;
    [SerializeField] private float timeForSwitch = 1f;

    [SerializeField] private float faseTime = 1.0f;
    private float t = 0f;

    private void Update()
    {
        if (luzFeijao.enabled)
        {
            luzFeijao.intensity = Mathf.PingPong(t * faseTime, 12f);
            t += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "VACA")
        {
            SwitchCowForBean();
        }
    }

    private void SwitchCowForBean()
    {
        StartCoroutine(SetActiveCow(false));
        StartCoroutine(SetActiveBean(true));
    }

    private IEnumerator SetActiveCow(bool active)
    {
        yield return new WaitForSeconds(timeForSwitch);
        if (active)
        {
            vaca.SetActive(true);
        }
        else
        {
            vaca.SetActive(false);
        }
    }
    private IEnumerator SetActiveBean(bool active)
    {
        yield return new WaitForSeconds(timeForSwitch + 0.5f);

        if (active)
        {
            feijao.SetActive(true);

            luzFeijao.enabled = active;
            yield return new WaitForSeconds(10.0f);
            luzFeijao.enabled = false;
        }
        else
        {
            feijao.SetActive(false);
        }
    }
}
