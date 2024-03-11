using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemHandler : MonoBehaviour
{
    public float gravity;
    float hours
        , minutes
        , seconds;
    enum GravityUnits { Newtons, MetersPerSecondSquared };
    enum RotationalAlgorithm { TwoVelocities, FixedStaticIntervals, FlexibleStaticIntervals };
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectsOfType<SystemHandler>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
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
