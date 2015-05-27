using UnityEngine;
using System.Collections;

public class GhostController : Mover {
	protected int target_x;
	protected int target_y;
	private GameObject pacman;

	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void setPoint() {
		target_x = (int)pacman.transform.position.x;
		target_y = (int)pacman.transform.position.y;
	}

	private void searchPath(int x, int y) {
		
	}

	void Update() {
		setPoint ();
		searchPath (target_x, target_y);
	}
}
