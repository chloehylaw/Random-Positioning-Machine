using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Common;
using TMPro;

public class JSONLoadSave : MonoBehaviour
{
    public JobRow jobRow;
    public TimeRow timeRow;
    public GravityRow gravityRow;
    public enum JobStatus {Running, Completed, Inactive}

    [SerializeField] private TMP_InputField inputJobName;
    [SerializeField] private TMP_InputField inputTimeDay;
    [SerializeField] private TMP_InputField inputTimeHour;
    [SerializeField] private TMP_InputField inputTimeMin;
    [SerializeField] private TMP_InputField inputTimeSec;
    [SerializeField] private TMP_InputField inputGravityValue;
    [SerializeField] private TMP_Dropdown inputGravityUnit;
    [SerializeField] private TMP_Dropdown inputRotationalAlgorithm;

    [System.Serializable]
    public class Job
    {
        public string jobName;
        public int timeDay;
        public int timeHour;
        public int timeMin;
        public int timeSec;
        public string gravityValue;
        public string gravityUnit;
        public string rotationalAlgorithm;
        public string status;
    }

    [System.Serializable]
    public class JobList
    {
        public Job[] job;
    }

    public JobList myJobList = new JobList();
    public Job myJob = new Job();
    
    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.dataPath + "/Data/data.json"))
        {
            string str = File.ReadAllText(Application.dataPath + "/Data/data.json");
            myJobList = JsonUtility.FromJson<JobList>(str);
        }
    }

    private bool ValidateJobName ()
    {
        // check if empty
        if (String.IsNullOrEmpty(inputJobName.text))
        {
            jobRow.WarningMessage("String is empty.");
            return false;
        }
        
        // check if name already exists
        if (myJobList.job.Any(j => j.jobName == inputJobName.text))
        {
            jobRow.WarningMessage("Job name already exists.");
            return false;
        }
        return true;
    }

    private bool ValidateTime ()
    {
        string day = inputTimeDay.text;
        string hour = inputTimeHour.text;
        string min = inputTimeMin.text;
        string sec = inputTimeSec.text;

        bool anyNotEmpty = !String.IsNullOrEmpty(day) || !String.IsNullOrEmpty(hour) || !String.IsNullOrEmpty(min) || !String.IsNullOrEmpty(sec);
        
        // if all inputs are empty
        if (!anyNotEmpty)
        {
            timeRow.WarningMessage("Empty time input.");
            return false;
        }
        
        // check if at least one is not 0
        bool anyNotZero = day != "0" || hour != "0" || min != "0" || sec != "0";
        
        // if empty or negative, set to 0
        if (string.IsNullOrEmpty(day) || day.Contains("-") || !int.TryParse(day, out _))
        {
            day = "0";
            timeRow.WarningMessage("Invalid day.");
            return false;
        }
            

        if (string.IsNullOrEmpty(hour) || hour.Contains("-") || !int.TryParse(hour, out _))
        {
            hour = "0";
            timeRow.WarningMessage("Invalid day.");
            return false;
        }

        if (string.IsNullOrEmpty(min) || min.Contains("-") || !int.TryParse(min, out _))
        {
            min = "0";
            timeRow.WarningMessage("Invalid day.");
            return false;
        }

        if (string.IsNullOrEmpty(sec) || sec.Contains("-") || !int.TryParse(sec, out _))
        {
            sec = "0";
            timeRow.WarningMessage("Invalid day.");
            return false;
        }
        
        return anyNotZero;
    }

    private bool ValidateGravityInput ()
    {
        string gravity = inputGravityValue.text;

        if (string.IsNullOrEmpty(gravity))
        {
            gravityRow.WarningMessage("Empty gravity input.");
            return false;
        }

        double number;
        if (!double.TryParse(gravity, out number))
        {
            gravityRow.WarningMessage("Invalid gravity input.");
            return false;
        }
        
        return number >= 0;
    }

    public void ValidateForm ()
    {
        if (!ValidateJobName() || !ValidateTime() || !ValidateGravityInput())
        {
            AddRecord();
        } else
        {
            Debug.Log("Refill form");
        }
    }

    // cant make it a list rip so array it is
    public void AddRecord ()
    {
        // collect data from input fields
        Job inputJob = new Job
        {
            jobName = inputJobName.text,
            timeDay = int.Parse(inputTimeDay.text),
            timeHour = int.Parse(inputTimeHour.text),
            timeMin = int.Parse(inputTimeMin.text),
            timeSec = int.Parse(inputTimeSec.text),
            gravityValue = inputGravityValue.text,
            gravityUnit = inputGravityUnit.options[inputGravityUnit.value].text,
            rotationalAlgorithm = inputRotationalAlgorithm.options[inputRotationalAlgorithm.value].text,
            status = JobStatus.Inactive.ToString()
        };

        // make temp array to hold old data and copy data into temp array
        JobList myJobListTemp = new JobList
        {
            job = myJobList.job
        };
        
        // make the original longer
        myJobList.job = new Job[myJobListTemp.job.Length + 1];

        // copy the old data back across
        for (int i = 0; i < myJobListTemp.job.Length; i++)
        {
            myJobList.job[i] = myJobListTemp.job[i];
        }
        
        // add new data
        myJobList.job[myJobListTemp.job.Length] = inputJob;
        
        // write data
        OutputJson();
    }

    void OutputJson ()
    {
        string str = JsonUtility.ToJson(myJobList,true);
        File.WriteAllText(Application.dataPath + "/Data/data.json", str);
    }

    public int FindRunningJobIndex ()
    {
        var runningJobIndex = Array.FindIndex(myJobList.job, j => j.status == JobStatus.Running.ToString());
        Debug.Log("The job with status 'Running' is at index: " + runningJobIndex); 
        return runningJobIndex;
    }

    public string RunningJobName ()
    {
        return myJobList.job[FindRunningJobIndex()].jobName;
    }

    public string RunningCurrentStatus ()
    {
        return myJobList.job[FindRunningJobIndex()].status;
    }

    public string RunningElapsedTime ()
    {
        return "running time";
    }

    public string RunningRemainingTime ()
    {
        return "remaining time";
    }
}
