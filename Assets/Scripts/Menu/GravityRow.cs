using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GravityRow : MonoBehaviour
{
    /// <summary>
    /// Desired gravity is in m/s^2
    /// </summary>
    public float DesiredGravity { get; set; }
    TMPro.TMP_InputField InputField;
    TMPro.TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        InputField = GetComponentInChildren<TMP_InputField>();
        dropdown = GetComponentInChildren<TMP_Dropdown>();
    }
    /// <summary>
    /// Verifies gravity value and converts it to m/s^2.
    /// </summary>
    /// <returns>True if gravity value is valid.</returns>
    public bool AttemptStart()
    {
        try
        {
            if (dropdown.GetComponentInChildren<TMPro.TMP_Text>().text == "g")
                DesiredGravity = float.Parse(InputField.text) * SystemHandler.instance.localG;
            else
                DesiredGravity = float.Parse(InputField.text);
            if (DesiredGravity > SystemHandler.instance.localG)
            {
                InputField.selectionColor = new Color(1, 0, 0);
                StartCoroutine(nameof(ColorFlash));
                return false;
            }
        }
        catch (FormatException)
        {
            InputField.selectionColor = new Color(1, 0, 0);
            StartCoroutine(nameof(ColorFlash));
            return false;
        }
        return true;
    }

    IEnumerator ColorFlash()
    {
        yield return new WaitForSeconds(3);
        InputField.selectionColor = new Color(0, 0, 0);
    }

}
