using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendinha : MonoBehaviour
{
    [SerializeField] private GameObject cow, bean;
    [SerializeField] private Light beanLight;
    [SerializeField] private float timeForSwitch = 1f;

    [SerializeField] private float faseTime = 1.0f;
    private float t = 0f;

    private void Update()
    {
        if (beanLight.enabled)
        {
            beanLight.intensity = Mathf.PingPong(t * faseTime, 12f);
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
            cow.SetActive(true);
        }
        else
        {
            cow.SetActive(false);
        }
    }
    private IEnumerator SetActiveBean(bool active)
    {
        yield return new WaitForSeconds(timeForSwitch + 0.5f);

        if (active)
        {
            bean.SetActive(true);

            beanLight.enabled = active;
            yield return new WaitForSeconds(10.0f);
            beanLight.enabled = false;
        }
        else
        {
            bean.SetActive(false);
        }
    }
}
