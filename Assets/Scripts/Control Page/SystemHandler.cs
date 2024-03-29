using Control_Page;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemHandler : MonoBehaviour
{
    /// <summary>
    /// Desired gravity in m/s^2
    /// </summary>
    public float gravity;
    public DateTime endDate;
    public RotationalAlgorithm algorithm;
    /// <summary>
    /// The interval to print to a file in seconds
    /// </summary>
    public float printInterval = 2f;
    public enum GravityUnits { Newtons, MetersPerSecondSquared };
    public static SystemHandler instance;
    public enum RotationalAlgorithm { TwoVelocities, FlexibleStaticIntervals, FixedStaticIntervals, None };
    /// <summary>
    /// Local gravity in m/s^2
    /// </summary>
    public float localG;
    public GameObject TwoVelocitiesPrefab;
    public GameObject FlexibleStaticIntervalsPrefab;
    public GameObject FixedStaticIntervalsPrefab;
    public GameObject currentScene;
    public Job EmptyJobPrefab;
    public Job currentJob;
    public enum CurrentJobStateEnum { None, Paused, Normal }
    public CurrentJobStateEnum currentJobState = CurrentJobStateEnum.None;

    // Start is called before the first frame update
    void Start()
    {
        localG = 9.81f;
        if(FindObjectsOfType<SystemHandler>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Directory.CreateDirectory(Application.dataPath + "\\Data");
    }

    public void HandleStop()
    {
        Destroy(currentScene);
        currentJob = EmptyJobPrefab;
        currentJobState = CurrentJobStateEnum.None;
    }

    public void HandleStart()
    {
        DataHandler.instance.CreateCSVFile();
        if (algorithm == RotationalAlgorithm.TwoVelocities)
            currentScene = Instantiate(TwoVelocitiesPrefab);
        else if (algorithm == RotationalAlgorithm.FlexibleStaticIntervals)
            currentScene = Instantiate(FlexibleStaticIntervalsPrefab);
        else if (algorithm == RotationalAlgorithm.FixedStaticIntervals)
            currentScene = Instantiate(FixedStaticIntervalsPrefab);
        currentJobState = CurrentJobStateEnum.Normal;
    }

}
