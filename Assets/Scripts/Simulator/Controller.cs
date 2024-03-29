using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float nominalRPM = 20f;
    public Motor outerMotor;
    public float outerMotorSpeed;
    public Motor innerMotor;
    public float innerMotorSpeed;
    protected List<Motor> motors;
    SerialPort port;
    protected bool paused = false;
    protected bool recordData = false;

    // Start is called before the first frame update
    protected void Start()
    {
        outerMotor.axisOfRotation = new Vector3(1, 0, 0);
        innerMotor.axisOfRotation = new Vector3(0, 1, 0);
        motors = new List<Motor>
        {
            outerMotor,
            innerMotor
        };
        outerMotor.SetSpeed(nominalRPM);
        innerMotor.SetSpeed(nominalRPM*Mathf.Sqrt(3)/2f);
        StartMotors(true);
        port = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);
        port.Open();
    }
    /// <summary>
    /// Starts all motors.
    /// </summary>
    /// <param name="resetInterval">Set to true if you wish to reset the random walk interval</param>
    protected void StartMotors(bool resetInterval = false)
    {
        foreach (Motor motor in motors)
        {
            motor.RandomWalk(resetInterval);
        }
    }
    /// <summary>
    /// Stops all motors.
    /// </summary>
    protected void StopMotors()
    {
        foreach (Motor motor in motors)
        {
            motor.Stop();
        }
    }

    /// <summary>
    /// Changes Nominal rpm.
    /// </summary>
    /// <param name="RPM">Speed in RPM</param>
    protected void SetNominalRPM(float RPM)
    {
        outerMotor.SetSpeed(RPM);
        innerMotor.SetSpeed(RPM * Mathf.Sqrt(3) / 2f);
        nominalRPM = RPM;
    }

    /// <summary>
    /// Changes motorspeed without editing nominalRPM
    /// </summary>
    /// <param name="RPM">Speed in RPM</param>
    protected void ChangeMotorSpeeds(float RPM)
    {
        outerMotor.SetSpeed(RPM);
        innerMotor.SetSpeed(RPM * Mathf.Sqrt(3) / 2f);
    }

    protected void FixedUpdate()
    {
        if (DateTime.Now.CompareTo(SystemHandler.instance.endDate) > 0)
        {
            StopMotors();
            SystemHandler.instance.HandleStop();
        }
        if (SystemHandler.instance.currentJobState == SystemHandler.CurrentJobStateEnum.Paused)
        {
            Pause();
        }
        if (outerMotorSpeed != outerMotor.currentSpeed || innerMotorSpeed != innerMotor.currentSpeed)
        {
            //port.WriteLine((outerMotor.currentSpeed / (360f * Time.fixedDeltaTime / 60f)).ToString() + ", " + (innerMotor.currentPosition / (360f * Time.fixedDeltaTime / 60f)).ToString());
        }
        outerMotorSpeed = outerMotor.currentSpeed;
        innerMotorSpeed = innerMotor.currentSpeed;
    }

    protected void Pause()
    {
        paused = true;
        StopMotors();
        recordData = false;
    }

    private void OnApplicationQuit()
    {
        port.Close();
    }
}
