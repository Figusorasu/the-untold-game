using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
            return;
        }

		Application.targetFrameRate = 60;
    }

	private PlayerControler player;
	[Header("PlayerStats")]
		[Range(0, 10)] public int p_health;
		[Range(0, 6)] public int p_ammo;
		[Range(0, 3)] public int p_sanity;
		[Space]
		public bool p_hasLamp = false;
		public bool p_hasGun = false;
		public bool p_gunEquipped = false;

	public void SavePlayerStats() {
		p_health = player.health;
		p_ammo = player.ammo;
		p_sanity = player.sanity;
		p_hasLamp = player.hasLamp;
		p_hasGun = player.hasGun;
		p_gunEquipped = player.gunEquipped;
	}

	public void LoadPlayerStats() {
		player.health = p_health;
		player.ammo = p_ammo;
		player.sanity = p_sanity;
		player.hasLamp = p_hasLamp;
		player.hasGun = p_hasGun;
		player.gunEquipped = p_gunEquipped;
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
	}

}
