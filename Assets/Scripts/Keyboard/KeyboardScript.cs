using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardScript : MonoBehaviour
{
    public InputField TextField;
    public GameObject EngLayoutSml, EngLayoutBig, EngLayoutSymb, FrLayoutSml, FrLayoutBig, FrLayoutSymb;

    public void alphabetFunction(string alphabet)
    {
        TextField.text = TextField.text + alphabet;
    }

    public void BackSpace()
    {
        if(TextField.text.Length>0) TextField.text = TextField.text.Remove(TextField.text.Length-1);
    }

    public void Delete ()
    {
        TextField.text = "";
    }

    private void CloseAllLayouts()
    {
        EngLayoutSml.SetActive(false);
        EngLayoutBig.SetActive(false);
        EngLayoutSymb.SetActive(false);
        FrLayoutSml.SetActive(false);
        FrLayoutBig.SetActive(false);
        FrLayoutSymb.SetActive(false);
    }

    public void ShowLayout(GameObject SetLayout)
    {
        Debug.Log(SetLayout);
        CloseAllLayouts();
        SetLayout.SetActive(true);
    }

}
