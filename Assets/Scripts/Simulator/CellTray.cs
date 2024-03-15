using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class CellTray : MonoBehaviour
{
	/// <summary>
	/// This is the fastest RPM the machine will operate at
	/// </summary>
	static readonly float nominalRPM = 20f;
	/// <summary>
	/// FixedUpdate should be set to 0.01s, so 100 ticks per second
	/// </summary>
	public float degreesPerTick;
    /// <summary>
    /// Number of ticks remaining for current instruction
    /// </summary>
    protected float commandTime;

    /// <summary>
    /// The minimum and maximum number of ticks until changing direction
    /// </summary>
    public static readonly int minimumWaitUntilDirectionChange = 15000;
    public static readonly int maximumWaitUntilDirectionChange = 60000;

    /// <summary>
    /// Speed as a factor of nominalRPM (0-1)
    /// </summary>
    public float innerSpeed, outerSpeed;
	/// <summary>
	/// Target position for Goto function
	/// </summary>
	public int desiredPosInner, desiredPosOuter;
	/// <summary>
	/// Position in degrees
	/// </summary>
	public float outerPos = 0, innerPos = 0;
	public bool goingTo = false;
	public bool stopping = false;
	public bool innerInverting = false, outerInverting = false;
	public bool innerInverted = false, outerInverted = false;
	public int innerInversionTimer, outerInversionTimer;
	public int innerInversionOperand = 0, outerInversionOperand = 0;
	public float innerSpeedMultiplier = (float)(1f / Math.Sqrt(5)); //kind of arbitrary irrational number just under 1

	// Start is called before the first frame update
	void Start()
	{
		CellTrayStart();
    }

    protected void CellTrayStart()
	{
		Time.timeScale = 10f;
		degreesPerTick = nominalRPM * (360f * Time.fixedDeltaTime/60f);
		innerInversionTimer = UnityEngine.Random.Range(minimumWaitUntilDirectionChange, maximumWaitUntilDirectionChange);
		outerInversionTimer = UnityEngine.Random.Range(minimumWaitUntilDirectionChange, maximumWaitUntilDirectionChange);
    }

	/// <summary>
	/// Moves the RPM for a given amount of ticks.
	/// </summary>
	/// <param name="innerSpeed">Speed as a proportion of nominalRPM</param>
	/// <param name="outerSpeed">Speed as a proportion of nominalRPM</param>
	/// <param name="ticks">Time in ticks to execute command</param>
	protected void Move(float innerSpeed, float outerSpeed, int ticks)
	{
		this.innerSpeed = innerSpeed;
		this.outerSpeed = outerSpeed;
		commandTime = ticks;
	}

	/// <summary>
	/// Specify a position for the RPM to rotate to. (0, 0) is default position
	/// </summary>
	/// <param name="posInner"></param>
	/// <param name="posOuter"></param>
	protected void GoTo(int posInner = 0, int posOuter = 0, bool stopping = false)
	{
		desiredPosInner = posInner;
		desiredPosOuter = posOuter;
		goingTo = true;
	}

	/// <summary>
	/// Stops the RPM and returns to default position.
	/// </summary>
	protected void Stop()
	{
		stopping = true;
		GoTo(0, 0, true);
	}

	protected void CellTrayFixedUpdate()
	{
        commandTime--;
		innerInversionTimer--;
		if (innerInversionTimer < 0)
		{
			innerInverting = true;
			innerInversionTimer = UnityEngine.Random.Range(minimumWaitUntilDirectionChange, maximumWaitUntilDirectionChange);
        }
		outerInversionTimer--;
		if (outerInversionTimer < 0)
		{
			outerInverting = true;
			outerInversionTimer = UnityEngine.Random.Range(minimumWaitUntilDirectionChange, maximumWaitUntilDirectionChange);
		}
        if (commandTime > 0 && !goingTo)
        {
            var tI = innerSpeed * degreesPerTick * innerSpeedMultiplier 
				* (innerInverting ? Mathf.Cos(innerInversionOperand++*0.005f*Mathf.PI) : 1f)
                * (innerInverted ? -1f : 1f);
			if(innerInversionOperand > 199)
			{
				innerInverting = false;
				innerInversionOperand = 0;
				innerInverted = !innerInverted;
			} 

            var tO = outerSpeed * degreesPerTick 
				* (outerInverting ? Mathf.Cos(outerInversionOperand++*0.005f*Mathf.PI) : 1f)
				* (outerInverted ? -1f : 1f);
            if (outerInversionOperand > 199)
            {
                outerInverting = false;
                outerInversionOperand = 0;
                outerInverted = !outerInverted;
            }

            transform.RotateAround(new Vector3(200, 100, 0), new Vector3(1, 0, 0), tO);
            outerPos = (outerPos + tO) % 360f;
            transform.Rotate(new Vector3(0, tI, 0));
            innerPos = (innerPos + tI) % 360f;
        }
        else if (goingTo)
        {
            if (stopping)
            {
                if(outerSpeed > 0)
				{
					outerSpeed = outerInverted ? outerPos/360f : -outerPos/360f;
				}
				if(outerSpeed < 0)
				{
					outerSpeed = 0;
				}
				if (innerSpeed > 0)
				{
                    innerSpeed = innerInverted ? innerPos / 360f : -innerPos / 360f; ;
				}
				if (innerSpeed < 0)
				{
					innerSpeed = 0;
				}
            }
            else
            {
                innerSpeed = innerSpeed > 0 ? innerSpeed : 1;
                outerSpeed = outerSpeed > 0 ? outerSpeed : 1;
            }
            bool outDone = false, inDone = false;
            if (outerPos != desiredPosOuter)
            {
                var tO = outerSpeed * degreesPerTick
                * (outerInverted ? -1f : 1f);
                transform.RotateAround(new Vector3(200, 100, 0), new Vector3(1, 0, 0), tO);
                outerPos = (outerPos + tO) % 360f;
            }
            else { outDone = true; }
            if (innerPos != desiredPosInner)
            {
                var tI = innerSpeed * degreesPerTick * innerSpeedMultiplier
                * (innerInverted ? -1f : 1f);
                transform.Rotate(new Vector3(0, tI, 0));
                innerPos = (innerPos + tI) % 360f;
            }
            else { inDone = true; }
            if (inDone && outDone)
            {
                goingTo = false;
            }
        }
    }

	void FixedUpdate()
	{
		//commandTime--;
		//if( commandTime > 0  )
		//{
		//	var tI = innerSpeed * degreesPerTick;
		//	var tO = outerSpeed * degreesPerTick;
		//	transform.RotateAround(new Vector3(200, 100, 0), new Vector3(1,0,0), tO);
		//	outerPos = (outerPos + tO) % 360f;
		//	transform.Rotate(new Vector3(0, tI, 0));
		//	innerPos += (innerPos + tI) % 360f;
		//}
		//else if (goingTo)
		//{
		//	if (stopping)
		//	{
  //              outerSpeed -= 0.05f;
		//		innerSpeed -= 0.05f;
  //          }
		//	else
		//	{
		//		innerSpeed = innerSpeed > 0 ? innerSpeed : 1;
		//		outerSpeed = outerSpeed > 0 ? outerSpeed : 1;
		//	}
		//	bool outDone = false, inDone = false;
		//	if (outerPos != desiredPosOuter)
		//	{
		//		var tO = outerSpeed * degreesPerTick;
		//		transform.RotateAround(new Vector3(200, 100, 0), new Vector3(1, 0, 0), tO);
		//		outerPos = (outerPos + tO) % 360f;
		//	}
		//	else { outDone = true; }
		//	if (innerPos != desiredPosInner)
		//	{
  //              var tI = innerSpeed * degreesPerTick;
  //              transform.Rotate(new Vector3(0, tI, 0));
  //              innerPos += (innerPos + tI) % 360f;
  //          }
		//	else { inDone = true; }
		//	if (inDone && outDone)
		//	{
		//		goingTo = false;
		//	}
  //      }
	}
}
