using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbButton : MonoBehaviour
{

    public bool actionButton = false;
    // Start is called before the first frame update
    public void OnClimbButton () {
        actionButton = true;
    }
}
