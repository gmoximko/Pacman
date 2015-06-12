using UnityEngine;
using System.Collections.Generic;

public class Akabei : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (27.0f, 34.0f);
		foodNeedToGo = GameManager.foodCount;
	}

	protected override void Chase() {
		target = (Vector2)pacman.transform.position;
		//Debug.Log ("TARGET Akabei: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}
}
