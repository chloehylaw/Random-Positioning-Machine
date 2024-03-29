using System;
using System.Collections;
using System.Collections.Generic;
using Control_Page;
using UnityEngine;

public class Job : MonoBehaviour
{
    public enum JobStatus {Running, Completed, Abort, None}

    
    public Guid guid;
    public string jobName;
    public float gravityValue;
    public SystemHandler.RotationalAlgorithm rotationalAlgorithm;
    public JobStatus status;
    public DateTime startTime;
    public DateTime endTime;
}
