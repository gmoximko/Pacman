﻿using UnityEngine;
using System.Collections;

public class Otoboke : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (0.0f, -1.0f);
	}

	protected override void Chase() {

		//Debug.Log ("TARGET Otoboke: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}
}
