using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPickup : MonoBehaviour, IInteractable {

    [SerializeField] private string prompt;
	public string InteractionPrompt => prompt;

	private PlayerControler player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
	}


	public void Interact() {
		Debug.Log("Interaction");
		player.hasLamp = true;
		Destroy(this.gameObject);
		
	}
}
