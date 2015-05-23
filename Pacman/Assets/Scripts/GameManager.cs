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
		foodGo ();
	}

	private void foodGo() {
		int i = 1;
		int j = 1;
		Stack<Vector2> queue = new Stack<Vector2> ();
		Vector2 insert = new Vector2 (i, j);
		int vertical;
		int horizontal;
		bool visit;
		Vector2 temp;

		queue.Push (insert);
	
		while (queue.Count > 0) {
			temp = queue.Pop();
			i = (int)temp.x;
			j = (int)temp.y;

			if (!foods.Contains(temp)) {
				Instantiate(food, temp, Quaternion.identity);
				foods.Add(temp);
			}

			do {
				visit = false;
				horizontal = 0;
				vertical = 0;

				insert = new Vector2(i + 1, j);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i - 1, j);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i, j + 1);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				insert = new Vector2(i, j - 1);
				setFood(ref visit, ref horizontal, ref vertical, ref queue, temp, insert);

				i += horizontal;
				j += vertical;

				temp = new Vector2(i, j);
			} while (visit);
		}
		Debug.Log (foods.Count);
	}

	private void setFood(ref bool visit, 
	                     ref int horizontal, 
	                     ref int vertical, 
	                     ref Stack<Vector2> queue, 
	                     Vector2 temp, 
	                     Vector2 insert) {
		if (!canFoodStay(insert)) {
			return;
		}

		if (Physics2D.Linecast(temp, insert, mask).transform == null) {
			if (!visit) {
				visit = true;
				if (     (temp - insert).x < 0) horizontal++;
				else if ((temp - insert).x > 0) horizontal--;
				else if ((temp - insert).y < 0) vertical++;
				else if ((temp - insert).y > 0) vertical--;
				Instantiate(food, insert, Quaternion.identity);
				foods.Add(insert);
			} else {
				queue.Push(insert);
			}
		} 
	}

	private bool canFoodStay(Vector2 insert) {

		if ((insert.x > columns - 1) 
			|| (insert.x < 0) 
			|| (insert.y > rows - 1) 
			|| (insert.y < 0)
			|| (insert.y > 10 && insert.y < 22 && insert.x != 6 && insert.x != 21)
			|| ((insert.x == 14 || insert.x == 13) && insert.y == 7)
		    || foods.Contains(insert)) {
			return false;
		}

		foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Energizer")) {
			Transform energizer = temp.GetComponent<Transform>();

			if (insert.x == energizer.position.x && insert.y == energizer.position.y) {
				return false;
			}
		}
		return true;
	}
}
