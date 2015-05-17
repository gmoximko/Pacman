using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private int rows = 28;
	private int columns = 31;
	public static GameManager gameManager = null;
	
	void Start () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
		initGame ();
	}

	private void initGame() {

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {

			}
		}
	}
	
	void Update () {
		
	}
}
