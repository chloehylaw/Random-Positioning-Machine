using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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

    public enum GravityUnits { Newtons, MetersPerSecondSquared };
    public static SystemHandler instance;
    public enum RotationalAlgorithm { TwoVelocities, FlexibleStaticIntervals, FixedStaticIntervals };
    /// <summary>
    /// Local gravity in m/s^2
    /// </summary>
    public float localG;
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
    }

    public void HandleStart()
    {
        if (algorithm == RotationalAlgorithm.TwoVelocities)
            SceneManager.LoadScene("Two Velocities");
        else if (algorithm == RotationalAlgorithm.FlexibleStaticIntervals)
            SceneManager.LoadScene("Flexible Static Intervals");
        else if (algorithm == RotationalAlgorithm.FixedStaticIntervals)
            SceneManager.LoadScene("Fixed Static Intervals");
    }

}
