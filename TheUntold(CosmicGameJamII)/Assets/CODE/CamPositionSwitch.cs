using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CamPositionSwitch : MonoBehaviour
{
    private GameObject cam;
    private Transform playerTR;

	[SerializeField] private Transform camTarget;

	private void Start()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		playerTR = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")){

			other.GetComponent<PlayerControler>().isInNewCamRegion = true;
			other.GetComponent<PlayerControler>().oldCamPos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

			cam.transform.position = camTarget.position;
			cam.transform.LookAt(playerTR);
		}
	}
	/*
	private void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player")){
			cam.transform.position = camTarget.position;
		}
	}*/



}
