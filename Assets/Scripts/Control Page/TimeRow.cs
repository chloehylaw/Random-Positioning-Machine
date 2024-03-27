using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRow : MonoBehaviour
{
    public DateTime DesiredDateEnd;
    TMP_InputField Days;
    TMP_InputField Hours;
    TMP_InputField Minutes;
    TMP_InputField Seconds;
    private TMP_Text text;
    public GameObject warning;
    public DateTime timeToCheckIfEntered;
    public event EventHandler OnUpdateDesiredEndDate;

    // Start is called before the first frame update
    void Start()
    {
        var t = GetComponentsInChildren<TMP_InputField>();
        Days = t[0];
        Hours = t[1];
        Minutes = t[2];
        Seconds = t[3];
        
        text = warning.GetComponent<TMP_Text>();
        text.text = "";
    }
    
    public void WarningMessage (String message)
    {
        Debug.Log(message);
        text.text = message;
    }

    public void UpdateDesiredDateEnd()
    {
        timeToCheckIfEntered = DateTime.Now;
        double.TryParse(Days.text, out double d);
        double.TryParse(Hours.text, out double h);
        double.TryParse(Minutes.text, out double m);
        double.TryParse(Seconds.text, out double s);
        DesiredDateEnd = DateTime.Now.AddDays(d);
        DesiredDateEnd = DesiredDateEnd.AddHours(h);
        DesiredDateEnd = DesiredDateEnd.AddMinutes(m);
        DesiredDateEnd = DesiredDateEnd.AddSeconds(s);
        OnUpdateDesiredEndDate.Invoke(this, EventArgs.Empty);
    }

    public bool AttemptStart()
    {
        return timeToCheckIfEntered != DesiredDateEnd;
    }

}
