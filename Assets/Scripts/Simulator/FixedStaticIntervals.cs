using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FixedStaticIntervals : Controller
{
	/// <summary>
	/// The total interval time in seconds
	/// </summary>
	float fullInterval = 30f;
	/// <summary>
	/// Interval of time spent not moving per full interval in seconds
	/// </summary>
	float staticInterval;
	/// <summary>
	/// Interval of time spent moving per full interval in seconds
	/// </summary>
	public float dynamicInterval;
    /// <summary>
    /// Desired gravity in proportion of g
    /// </summary>
    float g;
	public int tick;
	bool isStatic = false;

    new void Start()
	{
		base.Start();
        g = SystemHandler.instance.gravity / SystemHandler.instance.localG;
        dynamicInterval = fullInterval * g;
		staticInterval = fullInterval - dynamicInterval;
		foreach (Motor motor in motors)
		{
			motor.RandomWalk();
		}
		isStatic = false;
	}

    private new void FixedUpdate()	
    {
		if (isStatic)
		{
			tick++;
			if (tick >= staticInterval * (1f / Time.deltaTime))
			{
				StartMotors(false);
                isStatic = false;
				tick = 0;
			}
		}
		else
		{
			if (tick >= dynamicInterval * (1f / Time.deltaTime) && tick>0)
			{
				StopMotors();
				isStatic = true;
				tick = 0;
			}
			tick++;
		}
        base.FixedUpdate();
    }

}
