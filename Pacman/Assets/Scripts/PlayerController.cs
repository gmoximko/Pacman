using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	private int vertical = 0;
	private int horizontal = 0;

	protected override void Start () {
		base.Start();
		canMove = true;
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

		if ((vertical != 0 || horizontal != 0) && canMove) {
			move (horizontal, vertical);
		}
	}
}
