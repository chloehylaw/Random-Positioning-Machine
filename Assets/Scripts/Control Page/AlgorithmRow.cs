using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRow : MonoBehaviour
{
    public SystemHandler.RotationalAlgorithm DesiredAlgorithm;
    TMPro.TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponentInChildren<TMPro.TMP_Dropdown>();
        DesiredAlgorithm = GetAlgorithmFromDropdown();
    }

    SystemHandler.RotationalAlgorithm GetAlgorithmFromDropdown()
    {
        var t = dropdown.GetComponentInChildren<TMPro.TMP_Text>().text;
        if (t == dropdown.options[0].text)
        {
            return SystemHandler.RotationalAlgorithm.TwoVelocities;
        }
        else if (t == dropdown.options[1].text)
        {
            return SystemHandler.RotationalAlgorithm.FlexibleStaticIntervals;
        }
        else
        {
            return SystemHandler.RotationalAlgorithm.FixedStaticIntervals;
        }
    }

    public void OnDropdownChange()
    {
        DesiredAlgorithm = GetAlgorithmFromDropdown();
    }

    public bool AttemptStart()
    {
            return true;
    }
}
