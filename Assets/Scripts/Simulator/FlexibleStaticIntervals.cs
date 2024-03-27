using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleStaticIntervals : Controller
{
    /// <summary>
    /// The minimum interval time in seconds
    /// </summary>
    float minimumInterval = 30f;
    float g;
    float minimumTicks;
    public int tick;
    bool isStatic = false;
    public Accelerometer accelerometer;

    new void Start()
    {
        base.Start();
        g = SystemHandler.instance.gravity;
        ResetTicks();
        foreach (Motor motor in motors)
        {
            motor.RandomWalk();
        }
        isStatic = false;
    }

    private void ResetTicks()
    {
        minimumTicks = minimumInterval * (1 / Time.fixedDeltaTime);
    }
    

    private new void FixedUpdate()
    {
        minimumTicks--;
        base.FixedUpdate();
        if (accelerometer.currentAve < g * 1000f && !isStatic)
        {
            if (minimumTicks < 0)
            {
                foreach (Motor motor in motors)
                {
                    motor.Stop();
                }
                ResetTicks();
            }
        }
        else if (accelerometer.currentAve > g * 1000f && isStatic)
        {
            if(minimumTicks < 0)
            {
                foreach (Motor motor in motors)
                {
                    motor.RandomWalk();
                }
                ResetTicks();
            }
        }
    }
}
