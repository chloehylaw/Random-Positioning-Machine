using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FixedStaticIntervals : CellTray
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
	bool isStatic = false;

    void Start()
	{
        g = SystemHandler.instance.gravity / SystemHandler.instance.localG;
        dynamicInterval = fullInterval * g;
		staticInterval = fullInterval - dynamicInterval;
		CellTrayStart();
		Move(1, 1, (int)dynamicInterval * 100);
	}

    private void FixedUpdate()
    {
		if (commandTime == 0)
		{
			if (isStatic)
			{
                Move(1, 1, (int)dynamicInterval * 100);
				isStatic = false;
            }
			else
			{
				Stop();
				commandTime = staticInterval * 100;
				isStatic = true;
			}
        }
		CellTrayFixedUpdate();
    }

}
