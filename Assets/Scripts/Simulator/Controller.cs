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
        if (outerMotorSpeed != outerMotor.currentSpeed || innerMotorSpeed != innerMotor.currentSpeed)
        {
            //File.WriteAllText($"{Application.dataPath}/motorOutput.txt", outerMotor.currentSpeed.ToString() + ", " + innerMotor.currentSpeed.ToString());
            //Debug.Log(outerMotorSpeed);
        }
        outerMotorSpeed = outerMotor.currentSpeed;
        innerMotorSpeed = innerMotor.currentSpeed;
    }
}
