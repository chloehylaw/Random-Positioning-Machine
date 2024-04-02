using Control_Page;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class HomePage : MonoBehaviour
{
    private Job currentJob;
    private DateTime currentTime;

    public TMP_Text endDate;

    // home page info
    public TMP_Text jobText;

    public TMP_Text statusText;

    //public TMP_Text elapsedTimeText;
    public TMP_Text remainingTimeText;
    public TMP_Text expectedEndTimeText;

    private bool clickedPause;
    private bool clickedResume;
    private bool clickedStop;

    public void UpdateCurrentJob()
    {
        clickedPause = false;
        clickedResume = false;
        clickedStop = false;
        StartCoroutine(WaitForFrame());
    }

    IEnumerator WaitForFrame()
    {
        yield return new WaitForEndOfFrame();
        currentJob = SystemHandler.instance.currentJob;
    }

    void Start()
    {
        clickedPause = false;
        clickedResume = false;
        clickedStop = false;
    }
    void Update()
    {
        currentTime = DateTime.Now;

        if (currentJob != null)
        {
            //display
            jobText.text = currentJob.jobName;
            statusText.text = currentJob.status.ToString();
            //elapsedTimeText.text = currentTime.Subtract(startTime).ToString(@"dd\.hh\:mm\:ss");
            //elapsedTimeText.text = EvaluateElapsedTime();
            remainingTimeText.text = EvaluateRemainingTime();
            expectedEndTimeText.text = EvaluateExpectedEndTime();
        }
        else
        {
            //no current job
            //Debug.Log("no job");
        }
    }

    /// <summary>
    /// Pauses the current job. Saves the pause time
    /// </summary>
    public void ClickPauseButton()
    {
        Debug.Log("Paused");
        currentJob.pauseTime = DateTime.Now;
        clickedPause = true;
        clickedResume = false;
        SystemHandler.instance.HandlePause();
    }

    /// <summary>
    ///  Resumes the current job. Saves the resume time
    /// </summary>
    public void ClickResumeButton()
    {
        Debug.Log("Resumed");
        currentJob.resumeTime = DateTime.Now;
        clickedPause = false;
        clickedResume = true;
        SystemHandler.instance.HandleResume();
    }

    /// <summary>
    /// Stops the current job. Changes the status to abort and set the abort time
    /// </summary>
    public void ClickStopButton()
    {
        Debug.Log("Stopped");
        currentJob.status = Job.JobStatus.Abort;
        currentJob.abortTime = DateTime.Now;
        clickedStop = true;
        DataHandler.instance.UpdateCSV();
        SystemHandler.instance.HandleStop();
    }

    /// <summary>
    /// Calculates the elapsed time since the job has started
    /// </summary>
    /// <returns>The elapsed time</returns>
    private string EvaluateElapsedTime()
    {
        string newElapsedTime = null;
        DateTime startTime = Convert.ToDateTime(SystemHandler.instance.currentJob.startTime);

        if (clickedStop)
        {
            newElapsedTime = "";
        }
        else
        {
            newElapsedTime = currentTime.Subtract(startTime).ToString(@"dd\.hh\:mm\:ss");
        }

        return newElapsedTime;
    }

    /// <summary>
    /// Calculates the new remaining time if pause was clicked
    /// </summary>
    /// <returns>
    /// The new running time
    /// </returns>
    private string EvaluateRemainingTime()
    {
        DateTime expectedEndTime = Convert.ToDateTime(SystemHandler.instance.currentJob.expectedEndTime);
        string newRemainingTime = "";
        //Debug.Log(expectedEndTime);

        if (clickedPause)
        {
            newRemainingTime = "";
        }
        else if (clickedStop)
        {
            newRemainingTime = "";
        }
        else
        {
            newRemainingTime = expectedEndTime.Subtract(currentTime).ToString(@"dd\.hh\:mm\:ss");
        }
        return newRemainingTime;
    }

    /// <summary>
    /// Calculates the new end time once 
    /// </summary>
    /// <returns></returns>
    private string EvaluateExpectedEndTime()
    {
        string newExpectedEndTime = null;
        DateTime expectedEndTime = Convert.ToDateTime(SystemHandler.instance.currentJob.expectedEndTime);
        DateTime pauseTime = SystemHandler.instance.currentJob.pauseTime;
        DateTime resumeTime = SystemHandler.instance.currentJob.resumeTime;
        TimeSpan addedTime = resumeTime.Subtract(pauseTime);

        if (clickedResume)
        {
            newExpectedEndTime = expectedEndTime.Add(addedTime).ToString("G");
            DataHandler.instance.UpdateCSV();
        }
        else
        {
            newExpectedEndTime = SystemHandler.instance.currentJob.expectedEndTime.ToString("G");
        }

        return newExpectedEndTime;
    }

}