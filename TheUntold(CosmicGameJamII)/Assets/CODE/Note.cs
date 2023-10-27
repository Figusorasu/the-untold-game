using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, IInteractable {
	
	[SerializeField] private string prompt;
	[HideInInspector] public string InteractionPrompt => prompt;

	[SerializeField][Multiline] private string noteText;

	private UIController ui;

	private void Start()
	{
		ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
	}


	public void Interact() {
		ui.ShowNote(noteText);
	}

}
