using UnityEngine;
using System.Collections;

public abstract class Ghost : Mover {
	private delegate void Regime ();
	private Regime currentRegime;
	private Vector2 prevPos;
	private Vector2 currentPos;
	private readonly Vector2[] forbiddenUp = { new Vector2 (15.0f, 20.0f), 
											   new Vector2 (12.0f, 20.0f),
											   new Vector2 (12.0f,  8.0f),
											   new Vector2 (15.0f,  8.0f) };
	private readonly Vector2[] directions  = { new Vector2 (-1.0f, 0.0f), 
											   new Vector2 ( 1.0f, 0.0f),
		   									   new Vector2 ( 0.0f,-1.0f),
											   new Vector2 ( 0.0f, 1.0f) };

	protected GameObject pacman;
	protected Vector2 target;
	protected Vector2 scatterPoint;

	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
		prevPos = new Vector2 (15.0f, 19.0f); //идти вправо в самом начале запрещено
		GameManager.gameManager.ScatterRegime 	+= setScatterRegime;
		GameManager.gameManager.ChaseRegime 	+= setChaseRegime;
		GameManager.gameManager.FrightendRegime += setFrightendRegime;
	}
	
	private void Update() {
		
		if (canMove) {
			currentRegime();
			searchPath ((int)target.x, (int)target.y);
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			GameManager.gameManager.SendMessage("Restart");
		}
	}
	
	public void setScatterRegime() {
		currentRegime = new Regime (Scatter);
	}
	
	public void setChaseRegime() {
		currentRegime = new Regime (Chase);
	}
	
	public void setFrightendRegime() {
		currentRegime = new Regime (Frightend);
	}
	
	protected abstract void Chase ();
	
	private void Scatter() {
		target = scatterPoint;
	}
	
	private void Frightend() {
		Vector2 temp = new Vector2 (Random.Range (-1, 1), Random.Range (-1, 1));
		if (!GetComponent<Animator> ().GetBool("Frightend"))
			GetComponent<Animator> ().SetTrigger ("Frightend");

		if (temp.x != 0.0f) {
			temp.y = 0.0f;
		}
		target = currentPos + temp;
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
					if (temp == directions[3]) 
						chooseDir = temp;
					else if (temp == directions[0] 
					         && chooseDir != directions[3]) 
						chooseDir = temp;
					else if (temp == directions[2] 
					         && chooseDir != directions[3] 
					         && chooseDir != directions[0]) 
						chooseDir = temp;
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
		
		foreach (Vector2 temp in forbiddenUp) {
			if (temp == nextPos 
			    && currentPos - nextPos == new Vector2(0.0f, -1.0f)) {//идём наверх в запрещённых точках
				return false;
			}
		}
		return true;
	}
}
