using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {
	private float scatterTime;
	private float chaseTime;
	private float frightendTime;
	private float timer;
	private float t_timer;
	private string setRegime;
	private int wave;
	private Text[] texts;
	private Text betaInfo;
	private Text pause;
	private Image[] images;
	private int pacmanLives;
	private bool stopTimer;
	private const int wavesCount = 4;

	public int level { get; private set; }
	public const int foodCount = 240;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	public delegate void VoidFunc ();
	public event VoidFunc GamePaused;
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
		wave  = 0;
		timer = chaseTime;
		t_timer = 0.0f;
		setTimeFrightend (level, out frightendTime);
		setRegime = "scatter";
		pacmanLives = 2;
		stopTimer = false;
		texts = FindObjectsOfType<Text> ();
		images = FindObjectsOfType<Image> ();
		pause = (from temp in texts where temp.name == "Pause" select temp).First ();
		pause.enabled = false;
		GameManager.gameManager.GameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		GameStart ();
	}

	private void callFrightend() {
		StartCoroutine (frightendRegime());
		//textManager();
	}

	private void Restart() {
		//StopCoroutine (regimes);
		StopAllCoroutines ();
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		GameStart ();
		setRegime = "scatter";
		wave = 0;
		timer = chaseTime;
		setTimeFrightend (level, out frightendTime);
		texts = FindObjectsOfType<Text> ();
		images = FindObjectsOfType<Image> ();
		pause = (from temp in texts where temp.name == "Pause" select temp).First ();
		imagesManager ();
		//textManager ();
	}

	private void Update() {
		if (!stopTimer) {
			timer += Time.deltaTime;
		}

		if (Input.GetButtonDown ("Cancel")) {
			pacmanLives = 0;
			firstLevel();
		}

		if (Input.GetButtonDown ("Jump") && setRegime != "frightend") {
			stopTimer = !stopTimer;
			pause.enabled = !pause.enabled;
			GamePaused();
		}

		if (setRegime == "scatter" && timer >= chaseTime) {
			ScatterRegime();
			//textManager();
			timer = t_timer;//t_timer принимает значение только в режиме страха, чтобы узнать сколько времени длился
			t_timer = 0.0f; //предыдущий режим, после режима страха снова попадём в прежний режим и тот отыграет t_timer
			setRegime = "chase";

			if (timer == 0.0f) { //предполагается, что зашли сюда не из режима страха, тогда наступает следующая волна
				wave = (wave == wavesCount ? wave : wave + 1);
			}
			setTimeForRegimes(wave, level, out scatterTime, out chaseTime);
		} else if (setRegime == "chase" && timer >= scatterTime) {
			ChaseRegime();
			//textManager();
			timer = t_timer;
			t_timer = 0.0f;
			setRegime = "scatter";
		}
	}

	private IEnumerator frightendRegime() {
		string temp = setRegime;
		t_timer = timer;
		setRegime = "frightend";
		FrightendRegime ();
		yield return new WaitForSeconds (frightendTime);
		setRegime = (temp == "scatter" ? "chase" : "scatter");
		timer = (setRegime == "scatter" ? chaseTime : scatterTime);
	}

	private void firstLevel() {
		if (pacmanLives == 0) {
			level = 1;
			pacmanLives = 2;
			Restart();
		} else {
			GameObject.FindGameObjectWithTag("Player").SendMessage("pacmanHasLives");

			foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Ghost")) {
				temp.SendMessage("onGhostEaten");
			}
			pacmanLives--;
			images[pacmanLives].enabled = false;
		}
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

	private void textManager() {
		if (betaInfo == null) {
			betaInfo = (from temp in texts where temp.name == "Text" select temp).First();
		}
		betaInfo.text = "Level: " + level.ToString ();
		betaInfo.text += "\nWave: " + wave.ToString ();
		betaInfo.text += "\nRegime: " + setRegime;
		betaInfo.text += "\nChase time: " + chaseTime.ToString ();
		betaInfo.text += "\nScatter time: " + scatterTime.ToString ();
		betaInfo.text += "\nFrightend time: " + frightendTime.ToString ();
	}

	private void imagesManager() {
		(from temp in texts where temp.name == "Level" select temp).First ().text = "LEVEL: " + level.ToString ();
		pause.enabled = false;

		if (images.Length > pacmanLives) {
			for (int i = images.Length - 1; i >= pacmanLives; i--) {
				images[i].enabled = false;
			}
		}
	}
}
