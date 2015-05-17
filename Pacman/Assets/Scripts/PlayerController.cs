using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	public float speed;
	
	protected override void Start () {
		base.Start();
	}

	void Update () {
		int vertical = (int)Input.GetAxisRaw("Vertical");
		int horizontal = (int)Input.GetAxisRaw("Horizontal");

		if (vertical != 0) {
			horizontal = 0;
		}

		if (vertical != 0 || horizontal != 0) {
			Debug.Log(move (horizontal, vertical));
		}
	}
}
