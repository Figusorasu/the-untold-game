using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable {
	
	[SerializeField] private string prompt;
	public string InteractionPrompt => prompt;

	public bool loadLevel = false;
	public string levelName;

	private Animator anim;
	private bool doorOpen = false;
	private GameManager gm;

	private void Start() {
		anim = GetComponent<Animator>();
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	public void Interact() {
		Debug.Log("Interaction");
		if(!doorOpen) {
			anim.SetBool("DoorOpen", true);
			doorOpen = true;
			Debug.Log("Doors open");
			if(loadLevel) LoadLevel();
		} else {
			anim.SetBool("DoorOpen", false);
			doorOpen = false;
			Debug.Log("Doors closed");
		}
	}

	private void LoadLevel(){
		SceneManager.LoadScene(levelName);
	}
}
