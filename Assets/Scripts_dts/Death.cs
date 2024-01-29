using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
        DisableSelf();
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }

}
