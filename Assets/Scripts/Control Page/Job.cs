using System;
using System.Collections;
using System.Collections.Generic;
using Control_Page;
using UnityEngine;

public class Job : MonoBehaviour
{
    public enum JobStatus { Running, Completed, Abort, None }

    // handling meta data shown for user
    public Guid guid;
    public string jobName;
    public float gravityValue;
    public SystemHandler.RotationalAlgorithm rotationalAlgorithm;
    public JobStatus status;
    public DateTime startTime;
    public DateTime expectedEndTime;
    public DateTime endTime;
    public DateTime abortTime;

    // handling meta data for pause and resume
    public DateTime pauseTime;
    public DateTime resumeTime;
}
