using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{	

	private PlayerControler player;

	[SerializeField] private GameObject gameUI;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject interactionPrompt;
	[SerializeField] private TMP_Text interactionText;
	[SerializeField] private GameObject notePrompt;
	[SerializeField] private TMP_Text note;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Slider sanitySlider;
	[SerializeField] private Slider ammoSlider;

	public bool gameIsPaused;	

	public void PauseAndResume(){
		if(!gameIsPaused) {
			pauseMenu.SetActive(true);
			gameUI.SetActive(false);
			PauseGame();
		} else {
			pauseMenu.SetActive(false);
			gameUI.SetActive(true);
			ResumeGame();
		}
	}

	public void PauseGame() {
		Time.timeScale = 0;
		gameIsPaused = true;
	}
	
	public void ResumeGame() {
		Time.timeScale = 1;
		gameIsPaused = false;
	}

	public void ShowInteractionPrompt(string prompt) {
		interactionPrompt.SetActive(true);
		interactionText.text = prompt;
	}

	public void HideInteractionPrompt() {
		interactionPrompt.SetActive(false);
	}

	public void ShowNote(string noteText) {
		PauseGame();
		notePrompt.SetActive(true);
		note.text = noteText;
	}

	public void HideNote() {
		ResumeGame();
		notePrompt.SetActive(false);
	}

	public void GoToMenu() {
		SceneManager.LoadScene(0);
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
		pauseMenu.SetActive(false);
		gameUI.SetActive(true);
		Time.timeScale = 1;
		gameIsPaused = false;
	}

	private void Update()
	{
		healthSlider.value = player.health;
		sanitySlider.value = player.sanity;
		ammoSlider.value = player.ammo;
	}
}
