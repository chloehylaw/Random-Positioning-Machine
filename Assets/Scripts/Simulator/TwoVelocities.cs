using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoVelocities : Controller
{
    /// <summary>
    /// Desired gravity in gs
    /// </summary>
    float g;
    float speedFactorWhenInverted;
    public Accelerometer accelerometer;
    long tick;

    new void Start()
    {
        Time.timeScale = 2.0f;
        base.Start();
        g = SystemHandler.instance.gravity / SystemHandler.instance.localG;
        foreach (Motor motor in motors)
        {
            motor.RandomWalk();
        }
        speedFactorWhenInverted = g;
    }

    private new void FixedUpdate()
    {
        tick++;
        base.FixedUpdate();
        if (accelerometer.totalForce.y < 0)
        {
            ChangeMotorSpeeds(nominalRPM * speedFactorWhenInverted);
        }
        else
        {
            ChangeMotorSpeeds(nominalRPM);
        }
        if (tick % 6000 == 0)
        {
            speedFactorWhenInverted *= accelerometer.currentAve / (g*SystemHandler.instance.localG*1000f);
        }
    }
}
