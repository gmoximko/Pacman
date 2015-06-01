using UnityEngine;
using System.Collections;

public class PlayerController : Mover {
	private int vertical;
	private int horizontal;
	private int x; 
	private int y;
	private int foodEaten;
	public delegate void Regime();
	public event Regime FrightendRegime;

	protected override void Start () {
		base.Start();
		vertical = 0;
		horizontal = 0;
		x = -1;//default direction is left
		y = 0;
		foodEaten = 0;
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Food") {
			foodEaten++;

			if (GameManager.foodCount == foodEaten) {
				//GameManager.gameManager.Restart();
				GameManager.gameManager.SendMessage("Restart");
			}
		} else if (other.tag == "Energizer") {
			
		}
		other.gameObject.SetActive(false);
	}

	private void Update () {

		if ((int)Input.GetAxisRaw ("Vertical") != 0) {
			vertical = (int)Input.GetAxisRaw("Vertical");
			horizontal = 0;
		}

		if ((int)Input.GetAxisRaw ("Horizontal") != 0) {
			horizontal = (int)Input.GetAxisRaw("Horizontal");
			vertical = 0;
		}

		if (vertical != 0) {
			horizontal = 0;
		}

		if ((vertical != 0 || horizontal != 0)) {
			if (!move (horizontal, vertical)) {
				move(x, y); //don't change direction if path is locked 
			} else {
				x = horizontal;
				y = vertical;
			}
		}
	}

	private IEnumerator frightendRegime() {
		FrightendRegime ();
		yield return new WaitForSeconds (2.0f);
	}
}
