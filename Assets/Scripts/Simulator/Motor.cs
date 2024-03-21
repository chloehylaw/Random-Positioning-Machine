using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting.Python;
using UnityEngine;

public class Motor : MonoBehaviour
{
    /// <summary>
    /// Speed is in RPM, set by other classes. This represents the speed the user wants the rpm to spin at, regardless of direction.
    /// </summary>
    public float speed;
    /// <summary>
    /// Speed in deg per tick
    /// </summary>
    private float degreesPerTick;
    /// <summary>
    /// Speed that the motor wants to operate at, in degrees per tick
    /// </summary>
    public float desiredSpeed;
    /// <summary>
    /// Speed that the motor is currently operating at, in degrees per tick
    /// </summary>
    public float currentSpeed = 0;
    /// <summary>
    /// Flag for if machine wants to not move
    /// </summary>
    public bool stop;
    /// <summary>
    /// Amount of ticks until direction change
    /// </summary>
    public int randomWalkInterval;
    /// <summary>
    /// The axis of rotation for the gameObject to rotate about
    /// </summary>
    public Vector3 axisOfRotation;
    public float initialRotation;
    private Transform prevTransform;
    public bool tooCloseToStop = false;
    public float currentPosition = 0;
    private bool goingBackwards = false;
    public int minRandomWalkInterval = 15, maxRandomWalkInterval = 30;
    public bool waitForDirectionChange = false;

    public void Start()
    {
        degreesPerTick = speed * (360f * Time.fixedDeltaTime / 60f);
        desiredSpeed = degreesPerTick;
        initialRotation = Vector3.Dot(transform.localEulerAngles, axisOfRotation);
    }
    /// <summary>
    /// Returns to initial position
    /// </summary>
    public void Stop()
    {
        if (currentSpeed != desiredSpeed)
            waitForDirectionChange = true;
        desiredSpeed = 0;
        stop = true;
        if ((currentPosition < 90 || currentPosition > 270) && !waitForDirectionChange)
            tooCloseToStop = true;
    }

    /// <summary>
    /// Initiates movement. Spins constantly at degreesPerTick, periodically changing direction
    /// </summary>
    /// <param name="resetInterval">Set to true if you wish to reset the directing change interval</param>
    public void RandomWalk(bool resetInterval = false)
    {
        desiredSpeed = !goingBackwards ? degreesPerTick : -degreesPerTick;
        stop = false;
        if (resetInterval)
            randomWalkInterval = Random.Range(minRandomWalkInterval * (int)(1f / Time.fixedDeltaTime), maxRandomWalkInterval * (int)(1f / Time.fixedDeltaTime));
    }

    /// <summary>
    /// Set speed of rotation
    /// </summary>
    public void SetSpeed(float RPM)
    {
        if (RPM == 0f)
            Stop();
        else
        {
            speed = RPM;
            degreesPerTick = speed * (360f * Time.fixedDeltaTime / 60f);
        }
    }
    /// <summary>
    /// Reset random walk interval and change direction
    /// </summary>
    private void ChangeDirection()
    {
        randomWalkInterval = Random.Range(minRandomWalkInterval * (int)(1f / Time.fixedDeltaTime), maxRandomWalkInterval * (int)(1f / Time.fixedDeltaTime));
        desiredSpeed = -desiredSpeed;
        goingBackwards = !goingBackwards;
    }

    public void FixedUpdate()
    {
        if (!stop || waitForDirectionChange)
        {
            randomWalkInterval--;
            if (randomWalkInterval < 0)
            {
                ChangeDirection();
            }

            if (currentSpeed != desiredSpeed)
            {
                currentSpeed += 0.01f * Mathf.Sign(desiredSpeed - currentSpeed);
                if (Mathf.Abs(currentSpeed - desiredSpeed) < 0.02f)
                    currentSpeed = desiredSpeed;
            }
            if (waitForDirectionChange && currentSpeed == desiredSpeed)
            {
                waitForDirectionChange = false;
                if (currentPosition < 90 || currentPosition > 270)
                    tooCloseToStop = true;
            }
        }
        else
        {
            if (tooCloseToStop)
            {
                if (currentPosition > 90f && currentPosition < 270f)
                {
                    tooCloseToStop = false;
                }
            }
            else if (currentPosition < 90f || currentPosition > 270f)
            {
                if (Mathf.Abs(currentSpeed - desiredSpeed) <= 0.1f)
                {
                    if (currentPosition > 359f || currentPosition < 1f)
                    {
                        if (currentSpeed > 0)
                            currentSpeed = degreesPerTick * 0.01f;
                        else
                            currentSpeed = -degreesPerTick * 0.01f;
                        if (currentPosition >= 360f - (degreesPerTick * 0.005f) || currentPosition <= degreesPerTick * 0.005f)
                        {
                            currentPosition = 0;
                            currentSpeed = 0;
                        }

                    }
                    else
                    {
                        if (currentSpeed > 0)
                            currentSpeed = 0.1f;
                        else
                            currentSpeed = -0.1f;
                    }
                }
                else
                {
                    currentSpeed += (degreesPerTick / 90f) * Mathf.Sign(desiredSpeed - currentSpeed);
                }
            }
        }
        transform.Rotate(axisOfRotation, currentSpeed);
        UpdateMotorPosition();


    }

    private void UpdateMotorPosition()
    {
        currentPosition += currentSpeed;
        currentPosition %= 360f;
        if (currentPosition < 0f)
            currentPosition = 360f + currentPosition;
    }
}
