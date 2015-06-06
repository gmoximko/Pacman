using UnityEngine;
using System.Collections;

public class Pinky : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (1.0f, 34.0f);
	}

	protected override void Chase() {
		target = (Vector2)pacman.transform.position + pacman.GetComponent<PlayerController> ().dir * 4;
		//Debug.Log ("TARGET: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}
}
