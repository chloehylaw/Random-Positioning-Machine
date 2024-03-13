using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRow : MonoBehaviour
{
    public DateTime DesiredDateEnd;
    TMPro.TMP_InputField Days;
    TMPro.TMP_InputField Hours;
    TMPro.TMP_InputField Minutes;
    TMPro.TMP_InputField Seconds;

    public event EventHandler OnUpdateDesiredEndDate;

    // Start is called before the first frame update
    void Start()
    {
        var t = GetComponentsInChildren<TMPro.TMP_InputField>();
        Days = t[0];
        Hours = t[1];
        Minutes = t[2];
        Seconds = t[3];
    }

    public void UpdateDesiredDateEnd()
    {
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
        return DesiredDateEnd != null ? true : false;
    }

}
