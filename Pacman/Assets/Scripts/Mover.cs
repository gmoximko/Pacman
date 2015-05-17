using UnityEngine;
using System.Collections;

public abstract class Mover : MonoBehaviour {
	private Rigidbody2D body;
	private Collider2D coll;

	protected bool canMove;

	public float speed;
	public LayerMask mask;

	protected virtual void Start () {
		body = GetComponent<Rigidbody2D> ();
		coll = GetComponent<Collider2D> ();
	}

	protected bool move(int x, int y) {
		Vector2 start = (Vector2)transform.position;
		Vector2 end = new Vector2 (x, y) + start;
		RaycastHit2D hit;

		coll.enabled = false;
		hit = Physics2D.Linecast (start, end, mask);
		coll.enabled = true;

		if (hit.transform == null) {
			StartCoroutine(smoothMove(end));
			canMove = false;
			return true;
		}
		return false;
	}

	private IEnumerator smoothMove(Vector2 direction) {
		float sqrDistance = ((Vector2)transform.position - direction).sqrMagnitude;

		while (sqrDistance > float.Epsilon) {
			Vector2 newPos = Vector2.MoveTowards(body.position, direction, speed * Time.deltaTime);
			body.MovePosition(newPos);
			sqrDistance = ((Vector2)transform.position - direction).sqrMagnitude;
			yield return null;
		}
		canMove = true;
	}
}
