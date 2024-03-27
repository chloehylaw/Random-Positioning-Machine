using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
	// Start is called before the first frame update
	Vector3 prevPosition;
	Vector3 prevVelocity;
	Vector3 currentVelocity;
	Vector3 appliedGravity;
	public Vector3 totalForce;
	long tick = 0;
	long n = 0;
	/// <summary>
	/// Current average gravity in mm/s^2
	/// </summary>
	public float currentAve;

	private void Start()
	{
		prevPosition = transform.position;
		prevVelocity = Vector3.zero;
	}

	void FixedUpdate()
	{
		currentVelocity = (transform.position - prevPosition)*100;
		tick++;
		appliedGravity = Vector3.Scale(transform.up, new Vector3(1,-1,1)) * -9810;
		totalForce = prevVelocity - currentVelocity + appliedGravity;
		if (tick % 50 == 0)
		{
			n++;
			float force = Mathf.Sign(totalForce.y) * Mathf.Sqrt(totalForce.x * totalForce.x + totalForce.y * totalForce.y + totalForce.z * totalForce.z);
			if (n == 1)
				currentAve = force;
			else
			{
				currentAve = (((float)n - 1) * currentAve + force) / (float)n; 
			}
		}
		if (tick % (SystemHandler.instance.printInterval / Time.fixedDeltaTime) == 0)
		{
			string path = "Assets/test.txt";
			StreamWriter writer = new(path, true);
			writer.WriteLine(currentAve + ", ");
			writer.Close();
			Debug.Log(currentAve);
		}
		prevVelocity = currentVelocity;
		prevPosition = transform.position;
		if (tick == 360000)
		{
			Application.Quit();
		}
	}
}
