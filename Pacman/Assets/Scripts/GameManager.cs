using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	private const int foodCount = 240;
	public delegate void voidFunc ();

	public event voidFunc gameStart;
	[HideInInspector]public List<Vector2> foods;
	public BoardManager boardManager;
	public static GameManager gameManager = null;
	
	void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		GameManager.gameManager.gameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		foods = new List<Vector2> (foodCount);
		gameStart ();
	}

	public void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnLevelWasLoaded() {
		gameStart ();
	}
}
