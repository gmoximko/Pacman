using UnityEngine;
using System.Collections;

public class Pinky : Akabei {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (1.0f, 33.0f);
	}

	protected override void Chase() {

	}
}
