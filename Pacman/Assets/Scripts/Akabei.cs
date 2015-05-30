using UnityEngine;
using System.Collections.Generic;

public class Akabei : Mover {
	private GameObject pacman;
	private bool isMove = true;
	private Stack<Vector2> chooseDir;
	private readonly Vector2[] directions = { new Vector2 (-1.0f, 0.0f), 
										      new Vector2 ( 1.0f, 0.0f),
											  new Vector2 ( 0.0f,-1.0f),
											  new Vector2 ( 0.0f, 1.0f) };
	
	protected Vector2 target;

	
	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void setPoint() {
		target = (Vector2)pacman.transform.position;
	}

	private void searchPath(int x, int y) {
		Vector2 chooseDir = (Vector2)transform.position;
		Vector2 currentPos = chooseDir;
		float minDistance = 500.0f;

		foreach (Vector2 temp in directions) {
			if (Physics2D.Linecast(currentPos, currentPos + temp, mask).transform == null) {
				float currentDistance = Vector2.Distance(currentPos + temp, target);

				if (minDistance > currentDistance) {
					chooseDir = currentPos + temp;
					minDistance = currentDistance;
				}
			}
		}

		isMove = move (15, 19);
	}

	void Update() {

		if (isMove) {
			setPoint ();
			searchPath ((int)target.x, (int)target.y);
		}
	}
}
