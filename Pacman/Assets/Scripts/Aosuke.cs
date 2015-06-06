using UnityEngine;
using System.Collections;
using System.Linq;

public class Aosuke : Ghost {
	private GameObject akabei = null;

	protected override void Start() {
		base.Start ();
		scatterPoint = new Vector2 (28.0f, -1.0f);
	}

	protected override void Chase() {
		if (akabei == null) {
			akabei = (from temp in GameObject.FindGameObjectsWithTag ("Ghost") 
			          where temp.name == "Akabei(Clone)" 
			          select temp).First();
		}

		Vector2 akabeiPos = (Vector2)akabei.transform.position;
		Vector2 middlePoint = (Vector2)pacman.transform.position + pacman.GetComponent<PlayerController> ().dir * 2;
		float middleDistance = Vector2.Distance(akabeiPos, middlePoint);
		target = Vector2.Lerp (akabeiPos, middlePoint, middleDistance * 2);
		Debug.Log ("AKABEI: " + akabeiPos.ToString ());
		Debug.Log ("TARGET Aosuke: " + target.ToString() + " PACMAN " + ((Vector2)pacman.transform.position).ToString());
	}
}
