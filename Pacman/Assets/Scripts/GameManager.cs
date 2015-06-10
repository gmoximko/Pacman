using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private string setRegime;
	private Coroutine regimes;
	private int wave;

	public int level { get; private set; }
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
		frightendTime = 6.0f;
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
		wave = 1;
		setTimeFrightend (level, out frightendTime);
	}

	private void Update() {

		if (setRegime == "scatter") {
			setTimeForRegimes(wave, level, out scatterTime, out chaseTime);
			regimes = StartCoroutine (scatterRegime ());
		} else if (setRegime == "chase") {
			regimes = StartCoroutine (chaseRegime ());
		}
	}

	private IEnumerator scatterRegime() {
		setRegime = "stop_scatter";
		ScatterRegime ();
		//Debug.Log ("SCATTERTIME: " + scatterTime.ToString());
		yield return new WaitForSeconds (scatterTime);
		setRegime = "chase";
	}

	private IEnumerator chaseRegime() {
		setRegime = "stop_chase";
		ChaseRegime ();
		//Debug.Log ("CHASETIME: " + chaseTime.ToString());
		yield return new WaitForSeconds (chaseTime);
		setRegime = "scatter";
		wave = (wave == 4 ? wave : wave + 1);
		//Debug.Log ("WAVE " + wave.ToString());
	}

	private IEnumerator frightendRegime() {
		string temp = (setRegime == "" ? "scatter" : setRegime.Substring(5));
		setRegime = "stop_frightend";
		FrightendRegime ();
		//Debug.Log ("FRIGHTENDTIME: " + frightendTime.ToString());
		yield return new WaitForSeconds (frightendTime);
		setRegime = temp;
	}

	private void firstLevel() {
		level = 1;
		wave  = 1;
		frightendTime = 6.0f;
		setRegime = "scatter";
		Restart();
	}

	private void nextLevel() {
		level++;
		Restart();
	}

	private void setTimeFrightend(int level, out float frightendTime) {
		switch (level) {
		case  1: frightendTime = 6.0f; break;
		case  2: frightendTime = 5.0f; break;
		case  3: frightendTime = 4.0f; break;
		case  4: frightendTime = 3.0f; break;
		case  5: frightendTime = 2.0f; break;
		case  6: frightendTime = 5.0f; break;
		case  7: frightendTime = 2.0f; break;
		case  8: frightendTime = 2.0f; break;
		case  9: frightendTime = 1.0f; break;
		case 10: frightendTime = 5.0f; break;
		case 11: frightendTime = 2.0f; break;
		case 12: frightendTime = 1.0f; break;
		case 13: frightendTime = 1.0f; break;
		case 14: frightendTime = 3.0f; break;
		case 15: frightendTime = 1.0f; break;
		case 16: frightendTime = 1.0f; break;
		case 18: frightendTime = 1.0f; break;
		default: frightendTime = 0.0f; break;
		}
	}

	private void setTimeForRegimes(int wave, int level, 
	                               out float scatterTime, 
	                               out float chaseTime) {
		scatterTime = 0.0f;
		chaseTime = 0.0f;
		
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
		} else if (level > 1 && level < 5) {
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
	}
}
