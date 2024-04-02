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
    public string bufferedMessage;
    public int tick;

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

        port = new SerialPort("COM1", 4800);
        port.DtrEnable = true;
        port.RtsEnable = false;
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
        tick++;
        //should be refactored
        if (hasController && (controller.outerMotorSpeed != controller.outerMotor.currentSpeed || controller.innerMotorSpeed != controller.innerMotor.currentSpeed))
        {
            var outer = controller.outerMotor.currentSpeed*60f/(360f*Time.deltaTime);
            var inner = controller.innerMotor.currentSpeed*60f/(360f*Time.deltaTime);
            var outerPWM = 255 - (int)(Mathf.Abs(outer / 40f) * 255);
            var innerPWM = 255 - (int)(Mathf.Abs(inner / 40f) * 255);

            bufferedMessage = (Mathf.Sign(outer) > 0 ? "+" : "-") + outerPWM + ", " 
                + (Mathf.Sign(inner) > 0 ? "+" : "-") + innerPWM + '\n';
        }


        if (tick >= 0.1f/Time.fixedDeltaTime)
        {
            if (bufferedMessage != string.Empty)
            {
                byte[] byteMessage = System.Text.Encoding.UTF8.GetBytes(bufferedMessage);
                port.Write(byteMessage, 0, byteMessage.Length);
                tick = 0;
                bufferedMessage = string.Empty;
            }

        }
    }

    private void OnApplicationQuit()
    {
        port.Close();
    }
}
