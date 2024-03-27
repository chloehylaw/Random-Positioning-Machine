using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer_Tester : MonoBehaviour
{
    public Accelerometer a;
    public Accelerometer b;
    int tick = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tick++;
        //if(tick%100 == 0)
        //Debug.Log(a.totalForce - b.totalForce);
    }
}
