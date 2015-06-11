using UnityEngine;
using System.Collections.Generic;

public class Otoboke : Ghost {

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (0.0f, -1.0f);
	}

	protected override void Chase() {
		Vector2 start = (Vector2)transform.position;
		Vector2 pacmanPos = (Vector2)pacman.transform.position;
		int cells = cellsToPacman (start, pacmanPos);
		target = (cells > 8 ? pacmanPos : scatterPoint);
		//Debug.Log ("TARGET Otoboke: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}

	private int cellsToPacman(Vector2 start, Vector2 end) {
		Vector2 chooseDir;
		float minDistance;
		List<Vector2> visited = new List<Vector2> (9);//при пути до пакмана длинее восьми клеток- целью становится пакман

		while (start != end && visited.Count <= 8) {
			chooseDir = new Vector2(0.0f, 0.0f);
			minDistance = 500.0f;
			visited.Add(start);

			foreach (Vector2 temp in directions) {
				if (Physics2D.Linecast (start, start + temp, mask).transform == null 
				    //&& pathFree(start + temp) 
				    && !visited.Contains(start + temp)) {
					float currentDistance = Vector2.Distance (start + temp, end);

					if (minDistance > currentDistance) {
						chooseDir = temp;
						minDistance = currentDistance;
					}
				}
			}
			start += chooseDir;
			//Debug.Log(start.ToString());
		}
		return visited.Count;
	}
}
