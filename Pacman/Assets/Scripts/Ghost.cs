﻿using UnityEngine;
using System.Collections;

public abstract class Ghost : Mover {
	private delegate void Regime ();
	private Regime currentRegime;
	private bool isOutdoor;
	private int foodCount;
	private Vector2 prevPos;
	private Vector2 currentPos;
	private readonly Vector2 outDoorPos = new Vector2 (14.0f, 19.0f);
	private readonly Vector2[] forbiddenUp = { new Vector2 (15.0f, 20.0f), 
											   new Vector2 (12.0f, 20.0f),
											   new Vector2 (12.0f,  8.0f),
											   new Vector2 (15.0f,  8.0f) };
	protected readonly Vector2[] directions= { new Vector2 (-1.0f, 0.0f), 
									    	   new Vector2 ( 1.0f, 0.0f),
		   									   new Vector2 ( 0.0f,-1.0f),
											   new Vector2 ( 0.0f, 1.0f) };

	protected GameObject pacman;
	protected Vector2 target;
	protected Vector2 scatterPoint;
	protected int foodNeedToGo;

	public AudioClip pacmanDeath;
	public AudioClip ghostDeath;

	protected override void Start() {
		base.Start ();
		pacman = GameObject.FindGameObjectWithTag("Player");
		prevPos = new Vector2 (13.0f, 19.0f); //идти вправо в самом начале запрещено
		isOutdoor = false;
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().FoodEaten += onFoodEaten;
		GameManager.gameManager.ScatterRegime 	+= setScatterRegime;
		GameManager.gameManager.ChaseRegime 	+= setChaseRegime;
		GameManager.gameManager.FrightendRegime += setFrightendRegime;
	}

	private void Update() {
		if (isOutdoor && (Vector2)transform.position == startPos) {
			foodCount = 0;//счётчик еды обнуляется в случае если призрака съели и когда тот оказадся в стартовой позиции
			isOutdoor = false;
			coll.enabled = true;
		}

		if (canMove && currentRegime != null) {
			currentRegime();
			searchPath ((int)target.x, (int)target.y);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Tunnel") {
			speed = tunnelSpeed(GameManager.gameManager.level);
		}

		if (other.tag == "Player" && !frightend) {
			GameManager.gameManager.SendMessage ("firstLevel");
			source.clip = pacmanDeath;

			if (!source.isPlaying) {
				source.Play();
			}
		} else if (other.tag == "Player" && frightend) {
			onGhostEaten();
			source.clip = ghostDeath;

			if (!source.isPlaying) {
				source.Play();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Tunnel" && !frightend) {
			speed = setSpeed(GameManager.gameManager.level);
		}
	}

	private void OnDestroy() {
		//GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().FoodEaten -= onFoodEated;
		GameManager.gameManager.ScatterRegime 	-= setScatterRegime;
		GameManager.gameManager.ChaseRegime 	-= setChaseRegime;
		GameManager.gameManager.FrightendRegime -= setFrightendRegime;
		GameManager.gameManager.GamePaused 		-= onGamePaused;
	}

	private void setScatterRegime() {
		if (!frightend) {
			prevPos += (((Vector2)transform.position - prevPos).normalized * 2);
		} else {
			frightend = false;
			speed = setSpeed(GameManager.gameManager.level);
		}
		currentRegime = new Regime (Scatter);
	}
	
	private void setChaseRegime() {
		if (!frightend) {
			prevPos += (((Vector2)transform.position - prevPos).normalized * 2);	
		} else {
			frightend = false;
			speed = setSpeed(GameManager.gameManager.level);
		}
		currentRegime = new Regime (Chase);
	}
	
	private void setFrightendRegime() {
		prevPos += (((Vector2)transform.position - prevPos).normalized * 2);
		currentRegime = new Regime (Frightend);
		anim.SetTrigger ("Frightend");
		frightend = true;
		speed = frightendSpeed (GameManager.gameManager.level);
	}
	
	protected abstract void Chase ();
	
	private void Scatter() {
		target = scatterPoint;
	}
	
	private void Frightend() {
		Vector2 temp = new Vector2 (Random.Range (-1, 1), Random.Range (-1, 1));

		if (temp.x != 0.0f) {
			temp.y = 0.0f;
		}
		target = currentPos + temp;
	}

	private void ghostGoesOut() {

		if (moving != null) {
			StopCoroutine (moving);
		}
		StartCoroutine (smoothMove (outDoorPos));
		currentPos = outDoorPos;
		prevPos = new Vector2 (15.0f, 19.0f);
		isOutdoor = true;
	}

	private void onFoodEaten() {
		foodCount++;

		if (foodCount == foodNeedToGo) {
			ghostGoesOut();
		}
	}

	private void onGhostEaten() {
		coll.enabled = false;
		
		if (moving != null) {
			StopCoroutine(moving);
		}
		StartCoroutine (smoothMove (startPos));
		//coll.enabled = true;
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

	protected override float setSpeed(int level) {
		if (level == 1) {
			return speedValue * 0.75f;
		} else if (level > 1 && level < 5) {
			return speedValue * 0.85f;
		}
		return speedValue * 0.95f;
	}

	private float frightendSpeed(int level) {
		if (level == 1) {
			return speedValue * 0.5f;
		} else if (level > 1 && level < 5) {
			return speedValue * 0.55f;
		} else if ((level >= 5 && level <= 16) || level == 18) {
			return speedValue * 0.6f;
		} 
		return setSpeed(level);
	}

	private float tunnelSpeed(int level) {
		if (level == 1) {
			return speedValue * 0.4f;
		} else if (level > 1 && level < 5) {
			return speedValue * 0.45f;
		} 
		return speedValue * 0.5f;
	}
}
