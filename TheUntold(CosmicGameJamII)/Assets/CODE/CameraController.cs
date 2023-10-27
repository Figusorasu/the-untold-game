using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public bool DebugCamera;
	public Transform debugTarget;

	private Transform target;

	[Space]

	[SerializeField] private float smoothing;


	private void Start()
	{
		
		target = GameObject.FindGameObjectWithTag("CameraTarget").GetComponent<Transform>();

	}

	void FixedUpdate()
    {	
		if(DebugCamera){
			transform.position = debugTarget.position;
			transform.rotation = debugTarget.rotation;
		}else{
			Vector3 direction = target.position - transform.position;
			Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), smoothing * Time.deltaTime);
			transform.rotation = rotation;	
		}
		
    }

}
