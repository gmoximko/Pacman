using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Rigidbody2D body;
	public float speed;
	
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		if (vertical != 0.0f) {
			horizontal = 0.0f;
		}

		body.MovePosition (body.position + new Vector2 (horizontal, vertical) * speed * Time.deltaTime);
	}
}
