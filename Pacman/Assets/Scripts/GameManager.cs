using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private bool changeRegime;

	public const int foodCount = 240;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	public delegate void VoidFunc ();
	public event VoidFunc GameStart;
	public event VoidFunc ScatterRegime;
	public event VoidFunc ChaseRegime;
	public event VoidFunc FrightendRegime;

	private void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		scatterTime = 7.0f;
		chaseTime = 20.0f;
		frightendTime = 8.0f;
		changeRegime = true;
		GameManager.gameManager.GameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		GameStart ();
	}

	private void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		GameStart ();
		changeRegime = true;
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
