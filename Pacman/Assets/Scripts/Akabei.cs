using UnityEngine;
using System.Collections.Generic;

public class Akabei : Mover {
	private GameObject pacman;
	private Vector2 prevPos;
	private Vector2 currentPos;
	private readonly Vector2[] fodbiddenUp = { new Vector2 (15.0f, 20.0f), 
											   new Vector2 (12.0f, 20.0f),
											   new Vector2 (12.0f,  8.0f),
											   new Vector2 (15.0f,  8.0f) };
	private readonly Vector2[] directions  = { new Vector2 (-1.0f, 0.0f), 
										       new Vector2 ( 1.0f, 0.0f),
											   new Vector2 ( 0.0f,-1.0f),
											   new Vector2 ( 0.0f, 1.0f) };
	
	protected Vector2 target;
	
	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void setTarget() {
		target = (Vector2)pacman.transform.position;
	}

	private void searchPath(int x, int y) {
		Vector2 chooseDir = new Vector2(0.0f, 0.0f);
		float minDistance = 500.0f;
		currentPos = (Vector2)transform.position;

		foreach (Vector2 temp in directions) {
			if (Physics2D.Linecast(currentPos, currentPos + temp, mask).transform == null 
			    && pathFree(currentPos + temp)) {
				float currentDistance = Vector2.Distance(currentPos + temp, target);

				if (minDistance > currentDistance) {
					chooseDir = temp;
					minDistance = currentDistance;
				} else if (minDistance == currentDistance) {
					if      (temp == directions[3]) chooseDir = temp;
					else if (temp == directions[0] 
					         && chooseDir != directions[3]) chooseDir = temp;
					else if (temp == directions[2] 
					         && chooseDir != directions[3] 
					         && chooseDir != directions[0]) chooseDir = temp;
				}
			}
		}
		prevPos = currentPos;
		move ((int)chooseDir.x, (int)chooseDir.y);
	}

	private bool pathFree(Vector2 nextPos) {

		if (nextPos == prevPos) {
			return false;
		}

		foreach (Vector2 temp in fodbiddenUp) {
			if (temp == nextPos 
			    && currentPos - nextPos == new Vector2(0.0f, -1.0f)) {//идём наверх в запрещённых точках
				return false;
			}
		}
		return true;
	}

	void Update() {

		if (canMove) {
			setTarget ();
			searchPath ((int)target.x, (int)target.y);
		}
	}
}
