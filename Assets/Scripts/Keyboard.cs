using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    private System.Diagnostics.Process p;

    private void Update ()
    {
        try
        {
            if (p.HasExited)
            {
                Debug.Log("Process End");
            } else
            {
                Debug.Log("Process Running");
            }
        } catch (Exception rx)
        {
            
        }

    }
    public void OnBtnClick ()
    {
        try
        {
            // OSK
            p = System.Diagnostics.Process.Start("tabtip.exe"); //tabtip
        } catch (Exception e)
        {
            
        }
        
    }
}
