using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FixedStaticIntervals : Controller
{
	/// <summary>
	/// The total interval time in seconds
	/// </summary>
	float fullInterval = 150f;
	/// <summary>
	/// Interval of time spent not moving per full interval in seconds
	/// </summary>
	float staticInterval;
	/// <summary>
	/// Interval of time spent moving per full interval in seconds
	/// </summary>
	float dynamicInterval;
    /// <summary>
    /// Desired gravity in proportion of g
    /// </summary>
    float g;
	public int tick;
	bool isStatic = false;

    new void Start()
	{
		Time.timeScale = 2.0f;
		base.Start();
        g = SystemHandler.instance.gravity / SystemHandler.instance.localG;
        dynamicInterval = fullInterval * g;
		staticInterval = fullInterval - dynamicInterval;
		foreach (Motor motor in motors)
		{
			motor.RandomWalk();
		}
	}

    private new void FixedUpdate()	
    {
		base.FixedUpdate();
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
			tick++;
			if (tick >= dynamicInterval * (1f / Time.deltaTime))
			{
				StopMotors();
				isStatic = true;
				tick = 0;
			}
		}
    }

}
