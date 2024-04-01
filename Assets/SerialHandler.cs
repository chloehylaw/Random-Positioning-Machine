using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialHandler : MonoBehaviour
{
    Controller controller;
    SerialPort port;
    SerialPort port2;
    SerialPort port3;
    bool hasController = false;
    public static SerialHandler instance;
    public string bufferedMessage;
    public int tick;
    public int debug;

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

        port = new SerialPort("COM1", 4800, Parity.None, 8, StopBits.One);
        port2 = new SerialPort("COM2", 4800, Parity.None, 8, StopBits.One);
        port3 = new SerialPort("COM3", 4800, Parity.None, 8, StopBits.One);
        port.Open();
        port2.Open();
        port3.Open();
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
        tick++;
        //should be refactored
        if (hasController && (controller.outerMotorSpeed != controller.outerMotor.currentSpeed || controller.innerMotorSpeed != controller.innerMotor.currentSpeed))
        {
            var outer = controller.outerMotor.currentSpeed*60f/(360f*Time.deltaTime);
            var inner = controller.innerMotor.currentSpeed*60f/(360f*Time.deltaTime);
            var outerPWM = 255 - (int)(Mathf.Abs(outer / 40f) * 255);
            var innerPWM = 255 - (int)(Mathf.Abs(inner / 40f) * 255);

            bufferedMessage = (Mathf.Sign(outer) > 0 ? "+" : "-") + outerPWM + ", " 
                + (Mathf.Sign(inner) > 0 ? "+" : "-") + innerPWM;
        }


        if (tick >= 0.1f/Time.fixedDeltaTime)
        {
            if (bufferedMessage != "")
            {
                debug++;
                if(debug%3==0)
                    port.WriteLine(bufferedMessage);
                if (debug % 3 == 1)
                    port2.WriteLine(bufferedMessage);
                if (debug % 3 == 2)
                    port3.WriteLine(bufferedMessage);
                tick = 0;
                bufferedMessage = "";
            }

        }
    }

    private void OnApplicationQuit()
    {
        port.Close();
        port2.Close();
        port3.Close();
    }
}
