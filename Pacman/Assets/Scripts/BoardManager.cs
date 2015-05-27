using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	private const int rows = 31;
	private const int columns = 28;
	private readonly Vector2 pacmanPos = new Vector2 (14, 7);
	private readonly Vector2[] energizers = {new Vector2 ( 1,  7), 
											 new Vector2 (26,  7), 
											 new Vector2 (26, 27), 
											 new Vector2 ( 1, 27)};

	public LayerMask mask;
	public GameObject food;
	public GameObject energizer;
	public GameObject pacman;

	public void setLevel() {
		pacmanGo ();
		energizerGo ();
		foodGo ();
	}

	private void pacmanGo() {
		Instantiate (pacman, pacmanPos, Quaternion.identity);
	}

	private void energizerGo() {
		Instantiate (energizer, energizers[0], Quaternion.identity);
		Instantiate (energizer, energizers[1], Quaternion.identity);
		Instantiate (energizer, energizers[2], Quaternion.identity);
		Instantiate (energizer, energizers[3], Quaternion.identity);
	}

	private void foodGo() {
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
			
			if (!GameManager.gameManager.foods.Contains(temp)) {
				Instantiate(food, temp, Quaternion.identity);
				GameManager.gameManager.foods.Add(temp);
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
		Debug.Log (GameManager.gameManager.foods.Count);
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
				if (     (temp - insert).x < 0) horizontal++;
				else if ((temp - insert).x > 0) horizontal--;
				else if ((temp - insert).y < 0) vertical++;
				else if ((temp - insert).y > 0) vertical--;
				Instantiate(food, insert, Quaternion.identity);
				GameManager.gameManager.foods.Add(insert);
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
		    || (insert.y > 10 && insert.y < 22 && insert.x != 6 && insert.x != 21) //позиции где не должна быть еда
		    || ((insert.x == 14 || insert.x == 13) && insert.y == 7)               //позиция пакмана
		    || GameManager.gameManager.foods.Contains(insert)) {
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
