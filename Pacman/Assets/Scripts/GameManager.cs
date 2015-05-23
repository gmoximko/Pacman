using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	private const int rows = 31;
	private const int columns = 28;
	private const int foodCount = 240;
	private List<Vector2> foods;

	public LayerMask mask;
	public GameObject food;
	public static GameManager gameManager = null;
	
	void Start () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
		foods = new List<Vector2> (foodCount);
		//initGame ();
	}

	private void initGame() {
		int i = 1;
		int j = 1;
		int vertical;
		int horizontal;
		bool visit;
		Stack<Vector2> queue = new Stack<Vector2> ();
		Vector2 insert = new Vector2 (i, j);
		queue.Push (insert);
		foods.Add (insert);
		Instantiate(food, insert, Quaternion.identity);

		//while (queue.Count > 0) {
			Vector2 temp = queue.Pop();
			i = (int)temp.x;
			j = (int)temp.y;
		int exit = 0;
			do {
				visit = false;
				vertical = 0;
				horizontal = 0;
			exit++;
				insert = new Vector2(i + 1, j);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i - 1, j);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i, j + 1);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i, j - 1);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);
			Debug.Log("Horizontal: " + horizontal + " Vertical: " + vertical);
				i += horizontal;
				j += vertical;
			} while (exit < 10);
			//}
	}

	private void setFood(ref bool visit, 
	                     ref int horizontal, 
	                     ref int vertical, 
	                     ref Stack<Vector2> queue, 
	                     Vector2 temp, 
	                     Vector2 insert) {
		if (   (insert.x > columns - 1) 
		    || (insert.x < 0) 
		    || (insert.y > rows - 1) 
		    || (insert.y < 0)
		    || foods.Contains(insert)) {
			return;
		}

		if (Physics2D.Linecast(temp, insert, mask).transform == null) {
			if (!visit) {
				visit = true;
				if (     (temp - insert).x > 0)
					horizontal++;
				else if ((temp - insert).x < 0)
					horizontal--;
				else if ((temp - insert).y > 0)
					vertical++;
				else if ((temp - insert).y < 0)
					vertical--;
				Instantiate(food, insert, Quaternion.identity);
				foods.Add(insert);
			} else {
				queue.Push(insert);
				Debug.Log("I'M HERE");
			}
		} 
	}
}
