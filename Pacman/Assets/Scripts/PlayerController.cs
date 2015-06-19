using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	private int vertical;
	private int horizontal;
	private int x; 
	private int y;
	private int foodEaten;
	private int foodForAosuke;
	private int foodForOtoboke;
	private int foodForPinky;
	private bool playerEaten;
	private GameObject[] ghosts;

	public delegate void VoidFunc();
	public event VoidFunc FoodEaten;
	public AudioClip chomp;
	public AudioClip intermission;
	[HideInInspector]public Vector2 dir;

	protected override void Start () {
		base.Start();
		vertical = 0;
		horizontal = 0;
		x = -1;//default direction is left
		y = 0;
		foodEaten = 0;
		dir = new Vector2(x, y);
		ghosts = null;
		playerEaten = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Food") {

			if (!source.isPlaying) {
				source.clip = chomp;
				source.Play();
			}
			if (ghosts == null) {
				ghosts = GameObject.FindGameObjectsWithTag("Ghost");
			}
			foodEaten++;
			FoodEaten();
			if (GameManager.foodCount == foodEaten) {
				GameManager.gameManager.SendMessage("nextLevel");
			} 
			other.gameObject.SetActive(false);
		} else if (other.tag == "Energizer") {
			source.clip = intermission;

			if (!source.isPlaying) {
				source.Play();
			}
			GameManager.gameManager.SendMessage("callFrightend");
			other.gameObject.SetActive(false);
		}
	}

	private void Update () {
		if (playerEaten && (Vector2)transform.position == startPos) {
			coll.enabled = true;
		}

		if ((int)Input.GetAxisRaw ("Vertical") != 0) {
			vertical = (int)Input.GetAxisRaw("Vertical");
			horizontal = 0;
		}

		if ((int)Input.GetAxisRaw ("Horizontal") != 0) {
			horizontal = (int)Input.GetAxisRaw("Horizontal");
			vertical = 0;
		}

		if (vertical != 0) {
			horizontal = 0;
		}

		if ((vertical != 0 || horizontal != 0) && !GameManager.gameManager.isGamePaused) {
			if (!move (horizontal, vertical)) {
				move(x, y); //don't change direction if path is locked 
			} else {
				x = horizontal;
				y = vertical;
				dir = new Vector2(x, y);
			}
		}
	}

	private void OnDestroy() {
		GameManager.gameManager.GamePaused -= onGamePaused;
	}

	protected override float setSpeed(int level) {
		if (level == 1) {
			return speedValue * 0.8f;
		} else if ((level > 1 && level < 5) || level >= 21) {
			return speedValue * 0.9f;
		} 
		return speedValue;
	}

	private void pacmanHasLives() {
		coll.enabled = false;
		if (moving != null) {
			StopCoroutine(moving);
		}
		StartCoroutine(smoothMove (startPos));
		playerEaten = true;
		//coll.enabled = true;
	}
}
