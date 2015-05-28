using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	//[HideInInspector]public List<Vector2> foods;
	public const int foodCount = 240;
	public delegate void voidFunc ();
	public event voidFunc gameStart;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	
	void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		GameManager.gameManager.gameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		//foods = new List<Vector2> (foodCount);
		gameStart ();
	}

	public void Restart() {
		//foods = new List<Vector2> (foodCount); //?
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnLevelWasLoaded() {
		gameStart ();
	}
}
