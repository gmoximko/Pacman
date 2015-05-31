using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	private const int rows = 31;
	private const int columns = 28;
	private List<Vector2> foods;
	private readonly Vector2 pacmanPos = new Vector2 (14.0f, 7.0f);
	private readonly Vector2[] energizers = { new Vector2 ( 1.0f,  7.0f), 
											  new Vector2 (26.0f,  7.0f), 
											  new Vector2 (26.0f, 27.0f), 
											  new Vector2 ( 1.0f, 27.0f) };

	public LayerMask mask;
	public GameObject food;
	public GameObject energizer;
	public GameObject pacman;
	public Akabei[] ghosts;

	public void setLevel() {
		pacmanGo ();
		energizerGo ();
		foodGo ();
		ghostsGo ();
	}

	private void ghostsGo() {
		foreach (Akabei temp in ghosts) {
			Instantiate(temp);
		}
	}

	private void pacmanGo() {
		Instantiate (pacman, pacmanPos, Quaternion.identity);
	}

	private void energizerGo() {
		foreach (Vector2 temp in energizers) {
			Instantiate (energizer, temp, Quaternion.identity);	
		}
	}

	private void foodGo() {
		foods = new List<Vector2> (GameManager.foodCount); //?
		int i = 1; 
		int j = 1; //обход делаем начиная с первой клеточки лабиринта
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
				setFood(ref visit, ref horizontal, ref vertical, queue, temp, insert);
				
				insert = new Vector2(i - 1, j);
				setFood(ref visit, ref horizontal, ref vertical, queue, temp, insert);
				
				insert = new Vector2(i, j + 1);
				setFood(ref visit, ref horizontal, ref vertical, queue, temp, insert);
				
				insert = new Vector2(i, j - 1);
				setFood(ref visit, ref horizontal, ref vertical, queue, temp, insert);
				
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
	                     Stack<Vector2> queue, 
	                     Vector2 temp, 
	                     Vector2 insert) {
		if (!canFoodStay(insert)) {
			return;
		}
		
		if (Physics2D.Linecast(temp, insert, mask).transform == null) {
			if (!visit) {
				visit = true;
				if (     (temp - insert).x < 0.0f) horizontal++;
				else if ((temp - insert).x > 0.0f) horizontal--;
				else if ((temp - insert).y < 0.0f) vertical++;
				else if ((temp - insert).y > 0.0f) vertical--;
				Instantiate(food, insert, Quaternion.identity);
				foods.Add(insert);
			} else {
				queue.Push(insert);
			}
		} 
	}
	
	private bool canFoodStay(Vector2 insert) {
		
		if ((insert.x > columns - 1.0f) 
		    || (insert.x < 0.0f) 
		    || (insert.y > rows - 1.0f) 
		    || (insert.y < 0.0f)
		    || (insert.y > 10.0f && insert.y < 22.0f && insert.x != 6.0f && insert.x != 21.0f) //позиции где не должна быть еда
		    || ((insert.x == 14.0f || insert.x == 13.0f) && insert.y == 7.0f)               //позиция пакмана
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
