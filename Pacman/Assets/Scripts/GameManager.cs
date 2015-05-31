using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public const int foodCount = 240;
	public delegate void VoidFunc ();
	public event VoidFunc gameStart;
	public static GameManager gameManager = null;
	public BoardManager boardManager;

	private float scatterTime;
	private float chaseTime;
	private float frightendTime;

	private void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		scatterTime = 7.0f;
		chaseTime = 20.0f;
		frightendTime = 8.0f;
		GameManager.gameManager.gameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		gameStart ();
	}

	private void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		gameStart ();
	}

	private void Update() {

	}
}
