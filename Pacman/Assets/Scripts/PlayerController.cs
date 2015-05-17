using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	
	protected override void Start () {
		base.Start();
		canMove = true;
	}

	void Update () {
		int vertical = (int)Input.GetAxisRaw("Vertical");
		int horizontal = (int)Input.GetAxisRaw("Horizontal");

		if (vertical != 0) {
			horizontal = 0;
		}

		if ((vertical != 0 || horizontal != 0) && canMove) {
			Debug.Log(move (horizontal, vertical));
		}
	}
}
