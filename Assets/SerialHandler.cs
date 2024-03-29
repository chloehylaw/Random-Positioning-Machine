using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialHandler : MonoBehaviour
{
    Controller controller;
    SerialPort port;
    bool hasController = false;
    public static SerialHandler instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (FindObjectsOfType<SerialHandler>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(WaitTillEndOfFrame());

        port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        port.Open();
    }

    IEnumerator WaitTillEndOfFrame()
    {
        yield return new WaitForFixedUpdate();
        SystemHandler.instance.onControllerLoaded += LoadController;
        SystemHandler.instance.onControllerUnloaded += UnloadController;
    }

    void LoadController(Controller controller)
    {
        this.controller = controller;
        hasController = true;
    }

    void UnloadController(Controller controller)
    {
        hasController = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //should be refactored
        if (hasController && (controller.outerMotorSpeed != controller.outerMotor.currentSpeed || controller.innerMotorSpeed != controller.innerMotor.currentSpeed))
        {
            port.WriteLine((controller.outerMotor.currentSpeed / (360f * Time.fixedDeltaTime / 60f)).ToString() + ", " + (controller.innerMotor.currentPosition / (360f * Time.fixedDeltaTime / 60f)).ToString());
        }
    }

    private void OnApplicationQuit()
    {
        port.Close();
    }
}
