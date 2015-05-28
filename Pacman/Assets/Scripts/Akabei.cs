using UnityEngine;
using System.Collections;

public class Akabei : Mover {
	private GameObject pacman;
	private bool isMove = true;

	protected Vector2 left = new Vector2 (-1.0f, 0.0f);
	protected int target_x;
	protected int target_y;
	
	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void setPoint() {
		target_x = (int)pacman.transform.position.x;
		target_y = (int)pacman.transform.position.y;
	}

	private void searchPath(int x, int y) {
		int start_x = (int)transform.position.x;
		int start_y = (int)transform.position.y;

		isMove = move (x, y);
	}

	void Update() {

		if (isMove) {
			setPoint ();
			searchPath (target_x, target_y);
		}
	}
}
