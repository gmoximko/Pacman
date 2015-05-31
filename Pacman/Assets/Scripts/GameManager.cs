using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public const int foodCount = 240;
	public delegate void VoidFunc ();
	public event VoidFunc gameStart;
	public event VoidFunc ScatterRegime;
	public event VoidFunc ChaseRegime;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private bool changeRegime;
	private Akabei[] ghosts;

	private void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		scatterTime = 7.0f;
		chaseTime = 20.0f;
		frightendTime = 8.0f;
		changeRegime = false;
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
		if (changeRegime) {
			StartCoroutine(scatterRegime());
		}
	}

	private IEnumerator scatterRegime() {
		changeRegime = false;
		ScatterRegime ();
		yield return new WaitForSeconds (scatterTime);
		StartCoroutine (chaseRegime());
	}

	private IEnumerator chaseRegime() {
		ChaseRegime ();
		yield return new WaitForSeconds (chaseTime);
		changeRegime = true;
	}
}
