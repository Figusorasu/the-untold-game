using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
	[Header("Movement")]
		public float speed;

		private float moveInputHorizontal;
		private float moveInputVertical;
		private Vector3 moveInput;
		private Vector3 moveDirection;

		private bool canMove = true;

	[Header("PlayerStats")]
		[HideInInspector] public int maxHealth = 10;
		[Range(0, 10)] public int health;
		[HideInInspector] public int maxAmmo = 6;
		[Range(0, 6)] public int ammo;
		[HideInInspector] public int maxSanity = 3;
		[Range(0, 3)] public int sanity; // 0 - insane, 1 - mad, 2 - terrified, 3 - stable

		[Space]

		public bool hasLamp = false;
		public bool hasGun = false;

		[HideInInspector] public bool gunEquipped = false;
		

	[Header("Interaction System")]
		[SerializeField] private Transform interactorPoint;
		[SerializeField] private Vector3 interactorSize;
		[SerializeField] private LayerMask interactableMask;

		private readonly Collider[] interactableColliders = new Collider[3];
		private IInteractable interactable = null;

		public int _numFound;

	[Header("Components")]
		public Rigidbody _playerRB;
		public Transform _playerTR;

		[HideInInspector] public Vector3 oldCamPos;
		[HideInInspector] public Vector3 forwardDirection;
		[HideInInspector] public bool isInNewCamRegion;

		[SerializeField] private Transform playerGFX;
		[SerializeField] private Transform lookTarget;
		[SerializeField] private GameObject lamp;
		[SerializeField] private Animator anim;
		[SerializeField] private GameObject gun;

		private GameObject ui;
		private Camera cam;
		private InputManager _inputManager;
		private GameManager gm;
		

	#region Input Handler:
		private void Awake() {	
			ui = GameObject.FindGameObjectWithTag("UI");
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

			_inputManager = new InputManager();

			_inputManager.Player.Move.performed += ctx => moveInputHorizontal = ctx.ReadValue<Vector2>().x;
			_inputManager.Player.Move.performed += ctx => moveInputVertical = ctx.ReadValue<Vector2>().y;
			_inputManager.Player.Move.canceled += ctx => moveInputHorizontal = 0f;
			_inputManager.Player.Move.canceled += ctx => moveInputVertical = 0f;

			_inputManager.Player.Interaction.performed += ctx => InteractWithObject();
			_inputManager.Player.Pause.performed += ctx => ui.GetComponent<UIController>().PauseAndResume();
			//_inputManager.Player.EquipGun.performed += ctx => { if(!gunEquipped) { gunEquipped = true; } else { gunEquipped = false; }};
			//_inputManager.Player.Shoot.performed += ctx => Shoot();
			
		}
		private void OnEnable() {
			_inputManager.Player.Enable();
		}
		private void OnDisable() {
			_inputManager.Player.Disable();
		}
	#endregion

	private void Start()
	{
		gm.LoadPlayerStats();
	}

	private void Move() {
		moveDirection = (transform.forward * -moveInputVertical) + (transform.right * -moveInputHorizontal);
		_playerRB.MovePosition(_playerRB.position + moveDirection * speed * Time.fixedDeltaTime);
		
		if(moveInput != Vector3.zero){
			transform.rotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
		}


	}

	private void Look() {
		if(moveInput != Vector3.zero) {
			lookTarget.localPosition = -moveInput;
			var lookDirection = playerGFX.position - lookTarget.position;
			playerGFX.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
		} else {
			lookTarget.localPosition = Vector3.zero;
		}

	}

	private void UpdateAnimations() {
		if(moveInput != Vector3.zero) {
			anim.SetBool("isMoving", true);
		} else {
			anim.SetBool("isMoving", false);
		}

		if(gunEquipped) {
			anim.SetFloat("GunEquip", 1);
		} else {
			anim.SetFloat("GunEquip", 0);
		}

		if(gunEquipped) {
			gun.SetActive(true);
			gunEquipped = true;
		} else {
			gun.SetActive(false);
			gunEquipped = false;
		}

		if(hasLamp) {
			lamp.SetActive(true);
		} else {
			lamp.SetActive(false);
		}
		
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(cam.transform.position, new Vector3(transform.position.x, cam.transform.position.y, transform.position.z));
		Gizmos.DrawLine(playerGFX.position, lookTarget.position);
		Gizmos.DrawWireCube(interactorPoint.position, interactorSize);
	}
	
	private void InteractWithObject() {
		Debug.Log("Interaction Button Pressed");
		if(interactable != null) {
			canMove = false;
			anim.SetTrigger("Interaction");
			//await Task.Delay(958);
			interactable.Interact();
			canMove = true;
			
		}

		
	}

	private void Update()
	{
		// Check if in interaction box are any interactable objects
		var interactorHalfSize = new Vector3(interactorSize.x / 2, interactorSize.y / 2, interactorSize.z / 2);
		_numFound = Physics.OverlapBoxNonAlloc(interactorPoint.position, interactorHalfSize, interactableColliders, interactorPoint.rotation, interactableMask);
		if(_numFound > 0) {
			interactable = interactableColliders[0].GetComponent<IInteractable>();
			ui.GetComponent<UIController>().ShowInteractionPrompt(interactable.InteractionPrompt);
		} else {
			interactable = null;
			ui.GetComponent<UIController>().HideInteractionPrompt();
		}

		if(health > maxHealth) {
			health = maxHealth;
		}
		if(ammo > maxAmmo) {
			ammo = maxAmmo;
		}
		if(sanity > maxSanity) {
			sanity = maxSanity;
		}

		UpdateAnimations();
	}

	void FixedUpdate() {
		moveInput = new Vector3(moveInputHorizontal, 0f, moveInputVertical);
		
		//Change forward definition when entering new camera region only when player stops moving
		if(isInNewCamRegion == true) {
			if(moveInput != Vector3.zero) {
				forwardDirection = oldCamPos - (new Vector3(transform.position.x, oldCamPos.y, transform.position.z) + moveInput);
				
			} else {
				isInNewCamRegion = false;
			}
		} else {
			forwardDirection = cam.transform.position - (new Vector3(transform.position.x, cam.transform.position.y, transform.position.z) + moveInput);
		}
		if(canMove) {
			Move();
		}
		Look();

	}

}
