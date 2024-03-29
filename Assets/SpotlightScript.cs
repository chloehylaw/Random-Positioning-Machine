using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightScript : MonoBehaviour
{
    public Transform target;
    public Light light;
    public float rand;
    public float tick;


    private void Start()
    {
        light = gameObject.GetComponent<Light>();
        rand = Random.value;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        tick++;
        transform.LookAt(transform);
        transform.RotateAround(target.transform.position, new Vector3(0, 1, 0), Mathf.Sin(rand)*360f*Time.fixedDeltaTime/6f);
        var t = transform.position.normalized;
        light.color = new Color(Mathf.Sin(0.02f*tick*rand)*t.x, Mathf.Sin(0.02f * tick * rand * t.y), Mathf.Sin(0.01f * tick * rand)*t.z);
    }
}
