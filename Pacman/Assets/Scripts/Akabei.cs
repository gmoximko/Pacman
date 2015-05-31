using UnityEngine;
using System.Collections.Generic;

public class Akabei : Mover {
	private GameObject pacman;
	private Vector2 prevPos;
	private readonly Vector2[] directions = { new Vector2 (-1.0f, 0.0f), 
										      new Vector2 ( 1.0f, 0.0f),
											  new Vector2 ( 0.0f,-1.0f),
											  new Vector2 ( 0.0f, 1.0f) };
	
	protected Vector2 target;
	
	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
		prevPos = new Vector2(14.0f, 18.0f);
	}

	protected virtual void setTarget() {
		target = (Vector2)pacman.transform.position;
	}

	private void searchPath(int x, int y) {
		Vector2 chooseDir = new Vector2(0.0f, 0.0f);
		Vector2 currentPos = (Vector2)transform.position;
		float minDistance = 500.0f;

		foreach (Vector2 temp in directions) {
			if (Physics2D.Linecast(currentPos, currentPos + temp, mask).transform == null && currentPos + temp != prevPos) {
				float currentDistance = Vector2.Distance(currentPos + temp, target);

				if (minDistance > currentDistance) {
					chooseDir = temp;
					minDistance = currentDistance;
				}
			}
		}
		prevPos = currentPos;
		move ((int)chooseDir.x, (int)chooseDir.y);
	}

	void Update() {

		if (canMove) {
			setTarget ();
			searchPath ((int)target.x, (int)target.y);
		}
	}
}
