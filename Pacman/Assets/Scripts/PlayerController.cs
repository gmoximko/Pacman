using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerController : Mover {
	private int vertical;
	private int horizontal;
	private int x; 
	private int y;
	private int foodEaten;
	private GameObject[] ghosts;

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
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Food") {

			if (ghosts == null) {
				ghosts = GameObject.FindGameObjectsWithTag("Ghost");
			}
			foodEaten++;
			if (GameManager.foodCount == foodEaten) {
				GameManager.gameManager.SendMessage("nextLevel");
			} else if (foodEaten == 1) {
				findGhost("Pinky(Clone)").SendMessage("ghostGoesOut");
			} else if (foodEaten == 30) {
				findGhost("Aosuke(Clone)").SendMessage("ghostGoesOut");
			} else if (foodEaten == 120) {
				findGhost("Otoboke(Clone)").SendMessage("ghostGoesOut");
			}
		} else if (other.tag == "Energizer") {
			GameManager.gameManager.SendMessage("callFrightend");
		}
		other.gameObject.SetActive(false);
	}

	private void Update () {

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

		if ((vertical != 0 || horizontal != 0)) {
			if (!move (horizontal, vertical)) {
				move(x, y); //don't change direction if path is locked 
			} else {
				x = horizontal;
				y = vertical;
				dir = new Vector2(x, y);
			}
		}
	}

	protected override float setSpeed(int level) {
		if (level == 1) {
			return speedValue * 0.8f;
		} else if ((level > 1 && level < 5) || level >= 21) {
			return speedValue * 0.9f;
		} 
		return speedValue;
	}

	private GameObject findGhost(string name) {
		return (from temp in ghosts
		        where temp.name == name
		        select temp).First();
	}
}
