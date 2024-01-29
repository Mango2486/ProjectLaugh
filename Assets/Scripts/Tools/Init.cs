using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.height/3*4, Screen.currentResolution.height, false);
        Screen.SetResolution(Screen.currentResolution.height / 3 * 4, Screen.currentResolution.height, true);
    }
}
