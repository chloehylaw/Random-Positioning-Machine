using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityConversion : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public TMPro.TMP_Dropdown dropdown;

    private void Start()
    {
        text = gameObject.GetComponent<TMPro.TMP_Text>();
    }

    public void Convert()
    {
        if (dropdown.GetComponentInChildren<TMPro.TMP_Text>().text == "g")
        {
            text.text = (float.Parse(text.text) / SystemHandler.instance.localG).ToString();
        }
        else
        {
            text.text = (float.Parse(text.text) * SystemHandler.instance.localG).ToString();
        }
    }
}
