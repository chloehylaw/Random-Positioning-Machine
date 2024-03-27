using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndDateRow : MonoBehaviour
{
    public TimeRow timeRow;
    public TMP_Text endDate;
    public GameObject date;

    private void Start()
    {
        endDate = date.GetComponent<TMPro.TMP_Text>();
        //text = GetComponentInChildren<TMPro.TMP_Text>();
        timeRow.OnUpdateDesiredEndDate += UpdateTimeRow;
        endDate.text = "";
    }

    private void UpdateTimeRow(object sender, EventArgs e)
    {
        //text.text = "Expected end date: " + timeRow.DesiredDateEnd.ToString();
        endDate.text = timeRow.DesiredDateEnd.ToString();
    }
}

// I edit so the "Expected End Date" would always show