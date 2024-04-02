using Control_Page;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
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
    public Controller currentController;

    public delegate void AnswerCallback(Controller controller);
    public event AnswerCallback onControllerLoaded;
    public event AnswerCallback onControllerUnloaded;

    public int ScreensaverTicker = 0;
    /// <summary>
    /// Number of seconds of inactivity for screensaver
    /// </summary>
    public int ScreensaverTimer = 30;
    public int ScreensaverTimerInTicks;
    public CanvasGroup menu;


    public delegate void AnswerCallback(Controller controller);
    public event AnswerCallback onControllerLoaded;
    public event AnswerCallback onControllerUnloaded;

    public int ScreensaverTicker = 0;
    /// <summary>
    /// Number of seconds of inactivity for screensaver
    /// </summary>
    public int ScreensaverTimer = 30;
    public int ScreensaverTimerInTicks;
    public CanvasGroup menu;

    // Start is called before the first frame update
    void Start()
    {
        localG = 9.81f;
        if (FindObjectsOfType<SystemHandler>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Directory.CreateDirectory(Application.dataPath + "\\Data");
        currentJob = EmptyJobPrefab;
        ScreensaverTimerInTicks = (int)(ScreensaverTimer / Time.fixedDeltaTime);
        menu = FindObjectOfType<CanvasGroup>();
    }

    public void HandleStop(bool complete = false)
    {
        if (complete)
        {
            currentJob.status = Job.JobStatus.Completed;
            DataHandler.instance.CreateCSVFile();
        }
        currentJob = EmptyJobPrefab;
        currentJobState = CurrentJobStateEnum.None;
        Destroy(currentScene);
    }

    public void HandlePause()
    {
        currentController.StopMotors();
        currentJobState = CurrentJobStateEnum.Paused;
    }

    public void HandleResume()
    {
        currentController.StartMotors();
        currentJobState = CurrentJobStateEnum.Normal;
    }

    public void Update()
    {
        if (Input.anyKey)
        {
            ScreensaverTicker = 0;
            menu.alpha = 1;
            menu.interactable = true;
        }
    }

    public void FixedUpdate()
    {
        if (currentJobState == CurrentJobStateEnum.Normal)
            ScreensaverTicker++;
        else
            ScreensaverTicker = 0;
        if (ScreensaverTicker > ScreensaverTimerInTicks)
        {
            StartCoroutine(Fader());
            menu.interactable = false;
        }
    }

    public IEnumerator Fader()
    {
        float elapsedTime = 0;
        while (menu.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            menu.alpha = Mathf.Clamp01(1.0f - (elapsedTime / 3f));
            yield return null;
        }
        yield return null;
    }

    public void Update()
    {
        if (Input.anyKey)
        {
            ScreensaverTicker = 0;
            menu.alpha = 1;
            menu.interactable = true;
        }
    }

    public void FixedUpdate()
    {
        if (currentJobState == CurrentJobStateEnum.Normal)
            ScreensaverTicker++;
        else
            ScreensaverTicker = 0;
        if (ScreensaverTicker > ScreensaverTimerInTicks)
        {
            StartCoroutine(Fader());
            menu.interactable = false;
        }
    }

    public IEnumerator Fader()
    {
        float elapsedTime = 0;
        while (menu.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            menu.alpha = Mathf.Clamp01(1.0f - (elapsedTime / 3f));
            yield return null;
        }
        yield return null;
    }

    public void HandleStart()
    {
        DataHandler.instance.CreateCSVFile();
        if (algorithm == RotationalAlgorithm.TwoVelocities)
        {
            currentScene = Instantiate(TwoVelocitiesPrefab);
            currentController = FindObjectOfType<Controller>();
            onControllerLoaded(currentController);
        }
        else if (algorithm == RotationalAlgorithm.FlexibleStaticIntervals)
        {
            currentScene = Instantiate(FlexibleStaticIntervalsPrefab);
            currentController = FindObjectOfType<Controller>();
            onControllerLoaded(currentController);
        }
        else if (algorithm == RotationalAlgorithm.FixedStaticIntervals)
        {
            currentScene = Instantiate(FixedStaticIntervalsPrefab);
            currentController = FindObjectOfType<Controller>();
            onControllerLoaded(currentController);
        }
        currentJobState = CurrentJobStateEnum.Normal;
    }

}