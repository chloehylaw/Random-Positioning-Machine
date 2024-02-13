using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTray : MonoBehaviour
{
	int tick;
	float eulerx;
	float eulery;
	float eulerz;
	float upx;
	float upy;
	float upz;
	float nupx;
	float nupy;
	float nupz;
	float pnupx;
	float pnupy;
	float pnupz;
	Vector3 nup;
	Vector3 pnup;
	// Start is called before the first frame update
	void Start()
	{
		tick = 0;
		Time.timeScale = 1;

		//gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, -3, 0), new Vector3(60, 0, 0));
		//gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(8, 0, 0), new Vector3(0, 0, 30));
	}

	void FixedUpdate()
	{
		//eulerx = transform.eulerAngles.x;
		//eulery = transform.eulerAngles.y;
		//eulerz = transform.eulerAngles.z;
		//nup = Vector3.Scale(transform.up, new Vector3(1, -1, 1));
		//pnup = nup*9810;
		//upx = transform.up.x;
		//upy = transform.up.y;
		//upz = transform.up.z;
		//nupx = nup.x;
		//nupy = nup.y;
		//nupz = nup.z;
		//pnupx = pnup.x;
		//pnupy = pnup.y;
		//pnupz = pnup.z;

		tick++;
		transform.RotateAround(new Vector3(200, 100, 0), new Vector3(1,0,0), 0.3f);
		transform.Rotate(new Vector3(0, 0.3f, 0));
        
            
	}
}
