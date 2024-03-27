using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class JobRow : MonoBehaviour
{
    private TMP_Text text;
    public GameObject warning;

    private void Start ()
    {
        text = warning.GetComponent<TMP_Text>();
        text.text = "";
    }
    public void WarningMessage (String message)
    {
        Debug.Log(message);
        text.text = message;
    }
}
