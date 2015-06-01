using UnityEngine;
using System.Collections;

public class Pinky : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (1.0f, 34.0f);
	}

	protected override void Chase() {

	}
}
