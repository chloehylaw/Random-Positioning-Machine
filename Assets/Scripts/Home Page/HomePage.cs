using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HomePage : MonoBehaviour
{
    public JSONLoadSave json;

    private TMP_Text jobText;
    private TMP_Text statusText;
    private TMP_Text elapsedTimeText;
    private TMP_Text remainingTimeText;

    public GameObject jobName;
    public GameObject currentStatus;
    public GameObject elapsedTimeStatus;
    public GameObject remainingTimeStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        jobText = jobName.GetComponent<TMP_Text>();
        jobText.text = "";
        
        statusText = currentStatus.GetComponent<TMP_Text>();
        statusText.text = "";
        
        elapsedTimeText = elapsedTimeStatus.GetComponent<TMP_Text>();
        elapsedTimeText.text = "";
        
        remainingTimeText = remainingTimeStatus.GetComponent<TMP_Text>();
        remainingTimeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        jobText.text = json.RunningJobName();
        statusText.text = json.RunningCurrentStatus();
        elapsedTimeText.text = json.RunningElapsedTime();
        remainingTimeText.text = json.RunningRemainingTime();

    }
    
    
}
