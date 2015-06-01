using UnityEngine;
using System.Collections;

public class Aosuke : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (28.0f, -1.0f);
	}

	protected override void Chase() {
		
	}
}
