﻿using UnityEngine;
using System.Collections;

public abstract class Mover : MonoBehaviour {
	private Rigidbody2D body;
	private Collider2D coll;
	private Animator anim;
	private bool canMove;
	private readonly Vector2 rightTunnel = new Vector2 (27, 16);
	private readonly Vector2 leftTunnel  = new Vector2 (0, 16);

	public float speed;
	public LayerMask mask;

	protected virtual void Start () {
		body = GetComponent<Rigidbody2D> ();
		coll = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();
		canMove = true;
	}

	protected bool move(int x, int y) {

		if (!canMove) {
			return false;
		}
		Vector2 start = (Vector2)transform.position;
		Vector2 end = new Vector2 (x, y) + start;
		RaycastHit2D hit;

		coll.enabled = false;
		hit = Physics2D.Linecast (start, end, mask);
		coll.enabled = true;

		if (hit.transform == null) {
			setAnimation(end);
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

		if (body.position.x < leftTunnel.x) {
			body.MovePosition(rightTunnel);
		} else if (body.position.x >rightTunnel.x) {
			body.MovePosition(leftTunnel);
		}
		canMove = true;
	}

	private void setAnimation(Vector2 end) {
		Vector2 startPos = (Vector2)transform.position;

		if (startPos.x - end.x > 0) {
			anim.SetTrigger ("Left");
		} else if (startPos.x - end.x < 0) {
			anim.SetTrigger ("Right");
		} else if (startPos.y - end.y > 0) {
			anim.SetTrigger ("Down");
		} else if (startPos.y - end.y < 0) {
			anim.SetTrigger ("Up");
		}
	}
}
