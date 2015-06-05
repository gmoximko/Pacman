using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private string setRegime;
	private Coroutine regimes;

	public const int foodCount = 240;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	public delegate void VoidFunc ();
	public event VoidFunc GameStart;
	public event VoidFunc ScatterRegime;
	public event VoidFunc ChaseRegime;
	public event VoidFunc FrightendRegime;

	public void callFrightend() {
		StopCoroutine (regimes);
		StartCoroutine (frightendRegime());
	}

	private void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		scatterTime = 7.0f;
		chaseTime = 20.0f;
		frightendTime = 8.0f;
		setRegime = "scatter";
		GameManager.gameManager.GameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		GameStart ();
	}

	private void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		GameStart ();
		setRegime = "scatter";
	}

	private void Update() {

		if (setRegime == "scatter") {
			Debug.Log(setRegime);
			regimes = StartCoroutine (scatterRegime ());
		} else if (setRegime == "chase") {
			Debug.Log(setRegime);
			regimes = StartCoroutine (chaseRegime ());

		}
	}

	private IEnumerator scatterRegime() {
		setRegime = "stop_scatter";
		ScatterRegime ();
		yield return new WaitForSeconds (scatterTime);
		setRegime = "chase";
	}

	private IEnumerator chaseRegime() {
		setRegime = "stop_chase";
		ChaseRegime ();
		yield return new WaitForSeconds (chaseTime);
		setRegime = "scatter";
	}

	private IEnumerator frightendRegime() {
		string temp = setRegime.Substring(5);
		setRegime = "";
		FrightendRegime ();
		yield return new WaitForSeconds (frightendTime);
		setRegime = (temp == "" ? "scatter" : temp);
	}
}
