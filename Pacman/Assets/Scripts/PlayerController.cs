﻿using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	private int vertical;
	private int horizontal;
	private int x; 
	private int y;
	private int foodEaten;

	[HideInInspector]public Vector2 dir;

	protected override void Start () {
		base.Start();
		vertical = 0;
		horizontal = 0;
		x = -1;//default direction is left
		y = 0;
		foodEaten = 0;
		dir = new Vector2(x, y);
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Food") {
			foodEaten++;
			if (GameManager.foodCount == foodEaten) {
				GameManager.gameManager.SendMessage("nextLevel");
				GameManager.gameManager.SendMessage("Restart");
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
}
