using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public TimeRow timeRow;
    public GravityRow gravityRow;
    public AlgorithmRow algorithmRow;
    public JobRow jobRow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryStart()
    {
        if (timeRow.AttemptStart() &&  gravityRow.AttemptStart() && algorithmRow.AttemptStart() && jobRow.AttemptStart())
        {
            SystemHandler.instance.gravity = gravityRow.DesiredGravity;
            SystemHandler.instance.endDate = timeRow.DesiredDateEnd;
            SystemHandler.instance.algorithm = algorithmRow.DesiredAlgorithm;
            SystemHandler.instance.HandleStart();
        }
    }
}
