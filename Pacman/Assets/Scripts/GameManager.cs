using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private string setRegime;
	private Coroutine regimes;
	private int level;
	private int wave;

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
		level = 1;
		wave  = 1;
		setRegime = "scatter";
		GameManager.gameManager.GameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		GameStart ();
	}

	private void callFrightend() {
		StopCoroutine (regimes);
		regimes = StartCoroutine (frightendRegime());
	}

	private void Restart() {
		StopCoroutine (regimes);
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		GameStart ();
		setRegime = "scatter";
		level++;
		wave = 1;
	}

	private void Update() {

		if (setRegime == "scatter") {
			setTimeForRegimes(wave, level, out scatterTime, out chaseTime, out frightendTime);
			regimes = StartCoroutine (scatterRegime ());
		} else if (setRegime == "chase") {
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
		wave = (wave == 4 ? wave : wave + 1);
	}

	private IEnumerator frightendRegime() {
		string temp = (setRegime == "" ? "scatter" : setRegime.Substring(5));
		setRegime = "stop_frightend";
		FrightendRegime ();
		yield return new WaitForSeconds (frightendTime);
		setRegime = temp;
	}

	private void setTimeForRegimes(int wave, int level, 
	                               out float scatterTime, 
	                               out float chaseTime, 
	                               out float frightendTime) {
		scatterTime = 0.0f;
		chaseTime = 0.0f;
		frightendTime = 0.0f;
		
		if (level == 1) {
			switch (wave) {
			case 1:
				scatterTime = 7.0f;
				chaseTime = 20.0f;
				break;
			case 2:
				scatterTime = 7.0f;
				chaseTime = 20.0f;
				break;
			case 3:
				scatterTime = 5.0f;
				chaseTime = 20.0f;
				break;
			case 4:
				scatterTime = 5.0f;
				chaseTime = 10000.0f;
				break;
			}
		} else if (level >= 2 && level < 5) {
			switch (wave) {
			case 1:
				scatterTime = 7.0f;
				chaseTime = 20.0f;
				break;
			case 2:
				scatterTime = 7.0f;
				chaseTime = 20.0f;
				break;
			case 3:
				scatterTime = 5.0f;
				chaseTime = 1033.0f;
				break;
			case 4:
				scatterTime = 0.17f;
				chaseTime = 10000.0f;
				break;
			}
		} else if (level >= 5) {
			switch (wave) {
			case 1:
				scatterTime = 5.0f;
				chaseTime = 20.0f;
				break;
			case 2:
				scatterTime = 5.0f;
				chaseTime = 20.0f;
				break;
			case 3:
				scatterTime = 5.0f;
				chaseTime = 1037.0f;
				break;
			case 4:
				scatterTime = 0.17f;
				chaseTime = 10000.0f;
				break;
			}
		}
		frightendTime = 6.0f;
	}
}
