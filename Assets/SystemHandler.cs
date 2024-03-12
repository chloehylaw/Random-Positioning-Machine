using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemHandler : MonoBehaviour
{
    public float gravity;
    float hours
        , minutes
        , seconds;
    public enum GravityUnits { Newtons, MetersPerSecondSquared };
    public static SystemHandler instance;
    public enum RotationalAlgorithm { TwoVelocities, FixedStaticIntervals, FlexibleStaticIntervals };
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

    public void ChangeGravity(string gravity) { this.gravity = float.Parse(gravity); }
    public void ChangeHours(float hours) { this.hours = hours; }
    public void ChangeMinutes(float minutes) { this.minutes = minutes; }
    public void ChangeSeconds(float seconds) { this.seconds = seconds; }

    public void HandleStart()
    {

    }

    public void ChangeScene()
    {

    }
}
