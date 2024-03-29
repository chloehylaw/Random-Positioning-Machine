using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class HomePage : MonoBehaviour
{
    private Job currentJob;
    private DateTime currentTime;

    // home page info
    public TMP_Text jobText;
    public TMP_Text statusText;
    public TMP_Text elapsedTimeText;
    public TMP_Text remainingTimeText;

    public void UpdateCurrentJob()
    {
        currentJob = SystemHandler.instance.currentJob;
    }


    // Update is called once per frame
    void Update()
    {
        currentTime = DateTime.Now;

        if (currentJob != null)
        {
            DateTime startTime = Convert.ToDateTime(SystemHandler.instance.currentJob.startTime);
            DateTime endTime = Convert.ToDateTime(SystemHandler.instance.currentJob.endTime);
            
            //display
            jobText.text = currentJob.jobName;
            statusText.text = currentJob.status.ToString();
            elapsedTimeText.text = currentTime.Subtract(startTime).ToString(@"dd\.hh\:mm\:ss");
            remainingTimeText.text = endTime.Subtract(currentTime).ToString(@"dd\.hh\:mm\:ss");
            if (SystemHandler.instance.currentJob.rotationalAlgorithm == SystemHandler.RotationalAlgorithm.None && SystemHandler.instance.currentJobState == SystemHandler.CurrentJobStateEnum.None)
            {
                elapsedTimeText.text = "";
                remainingTimeText.text = "";
            }
        }
        else
        {
            currentJob = SystemHandler.instance.EmptyJobPrefab;

        }
    }
}
