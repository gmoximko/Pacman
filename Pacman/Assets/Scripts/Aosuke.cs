using UnityEngine;
using System.Collections;
using System.Linq;

public class Aosuke : Ghost {
	private GameObject akabei = null;

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (28.0f, -1.0f);
		foodNeedToGo = (int)(GameManager.foodCount * 0.2) - GameManager.gameManager.level;
	}

	protected override void Chase() {
		if (akabei == null) {
			akabei = (from temp in GameObject.FindGameObjectsWithTag ("Ghost") 
			          where temp.name == "Akabei(Clone)" 
			          select temp).First();
		}

		Vector2 akabeiPos = (Vector2)akabei.transform.position;
		Vector2 middlePoint = (Vector2)pacman.transform.position + pacman.GetComponent<PlayerController> ().dir * 2;
		middlePoint -= (akabeiPos - middlePoint);
		target = middlePoint;
		//Debug.Log ("TARGET Aosuke: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}
}
