using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	private int vertical = 0;
	private int horizontal = 0;
	private int x = -1; //default direction is left
	private int y = 0;
	private int foodEaten = 0;

	protected override void Start () {
		base.Start();
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Food") {
			foodEaten++;

			if (GameManager.foodCount == foodEaten) {
				GameManager.gameManager.Restart();
			}
		} else if (other.tag == "Energizer") {
			
		}
		other.gameObject.SetActive(false);
	}

	void Update () {

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
			}
		}
	}
}
