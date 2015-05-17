using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public GameManager gameManager;

	void Start() {

		if (GameManager.gameManager == null) {
			Instantiate(gameManager);
		}
	}

}
