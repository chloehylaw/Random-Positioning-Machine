using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDateRow : MonoBehaviour
{
    public TimeRow timeRow;
    TMPro.TMP_Text text;
    public GameObject date;

    private void Start()
    {
        text = date.GetComponent<TMPro.TMP_Text>();
        //text = GetComponentInChildren<TMPro.TMP_Text>();
        timeRow.OnUpdateDesiredEndDate += UpdateTimeRow;
        text.text = "";
    }

    public void UpdateTimeRow(object sender, EventArgs e)
    {
        //text.text = "Expected end date: " + timeRow.DesiredDateEnd.ToString();
        text.text = " " + timeRow.DesiredDateEnd.ToString();
    }
}

// I edit so the "Expected End Date" would always show