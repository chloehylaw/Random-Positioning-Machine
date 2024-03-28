using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboard : MonoBehaviour
{
    private System.Diagnostics.Process p;

    /*private void Update()
    {
        if (p.HasExited)
        {
            Debug.Log("Process end");
        }
        else
        {
            Debug.Log("Process running");
        }
    }*/

    public void OnButtonClick()
    {
        System.Diagnostics.Process.Start("OSK.exe");
    }
}
