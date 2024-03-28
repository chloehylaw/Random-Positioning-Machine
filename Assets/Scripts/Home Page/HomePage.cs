using System;
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
            //DateTime startTime = Convert.ToDateTime(SystemHandler.instance.currentJob.startTime);
            //DateTime expectedEndTime = Convert.ToDateTime(SystemHandler.instance.currentJob.endTime);
            //Debug.Log(expectedEndTime);

            //display
            jobText.text = currentJob.jobName;
            statusText.text = currentJob.status.ToString();
            //elapsedTimeText.text = currentTime.Subtract(startTime).ToString(@"dd\.hh\:mm\:ss");
            //elapsedTimeText.text = EvaluateElapsedTime();
            //remainingTimeText.text = expectedEndTime.Subtract(currentTime).ToString(@"dd\.hh\:mm\:ss");
            remainingTimeText.text = EvaluateRemainingTime();
            expectedEndTimeText.text = EvaluateExpectedEndTime();
            //expectedEndTimeText.text = SystemHandler.instance.currentJob.endTime.ToString("G");
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
        currentJob.pauseTime = DateTime.Now;
        clickedPause = true;
        clickedResume = false;
    }

    /// <summary>
    ///  Resumes the current job. Saves the resume time
    /// </summary>
    public void ClickResumeButton()
    {
        currentJob.resumeTime = DateTime.Now;
        clickedPause = false;
        clickedResume = true;
    }

    /// <summary>
    /// Stops the current job. Changes the status to abort and set the abort time
    /// </summary>
    public void ClickStopButton()
    {
        currentJob.status = Job.JobStatus.Abort;
        currentJob.abortTime = DateTime.Now;
        clickedStop = true;
        UpdateCSV();
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
        string newRemainingTime = null;
        DateTime expectedEndTime = Convert.ToDateTime(SystemHandler.instance.currentJob.endTime);

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

        if (expectedEndTime.Equals(DateTime.MinValue))
        {
            //Debug.Log("done");
            currentJob.status = Job.JobStatus.Completed;
            newRemainingTime = "";
            currentJob.endTime = DateTime.Now;
            //endDate.text = "End Date: " + DateTime.Now.ToString("G");
            UpdateCSV();
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
            UpdateCSV();
        }
        else
        {
            newExpectedEndTime = SystemHandler.instance.currentJob.expectedEndTime.ToString("G");
        }

        return newExpectedEndTime;
    }

    /// <summary>
    /// Update the CSV file
    /// </summary>
    private void UpdateCSV()
    {
        string[] jobData =
        {
            currentJob.guid.ToString(),
            currentJob.jobName,
            currentJob.gravityValue.ToString(CultureInfo.InvariantCulture),
            currentJob.rotationalAlgorithm.ToString(),
            currentJob.status.ToString(),
            currentJob.startTime.ToString(CultureInfo.InvariantCulture),
            currentJob.expectedEndTime.ToString(CultureInfo.InvariantCulture),
            currentJob.endTime.ToString(CultureInfo.InvariantCulture),
            currentJob.abortTime.ToString(CultureInfo.InvariantCulture)
        };

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Join(",", jobData));

        string pathName = Application.dataPath + "/Data/" +
                          currentJob.startTime.ToString("yy-MM-dd HH-mm-ss").Replace(" ", "_") +
                          "_" + currentJob.jobName + ".csv";

        string[] lines = File.ReadAllLines(pathName);
        lines[1] = string.Join(",", jobData);
        File.WriteAllLines(pathName, lines);
    }
}