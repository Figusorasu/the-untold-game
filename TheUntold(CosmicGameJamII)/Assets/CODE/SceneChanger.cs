using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class SceneChanger : MonoBehaviour, IInteractable
{
    private GameManager _gm;

	[SerializeField] private string prompt;
	public string InteractionPrompt => prompt;

	[SerializeField] private string sceneName;

	private void Start()
	{
		_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	private bool SceneExist() {
		return SceneManager.GetSceneByName(sceneName).IsValid();
	}

	private void LoadScene() {

			_gm.SavePlayerStats();
			SceneManager.LoadScene(sceneName);

	}

	public void Interact() {
		LoadScene();
	}


}
