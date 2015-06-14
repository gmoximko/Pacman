using UnityEngine;
using System.Collections;

public abstract class Mover : MonoBehaviour {
	private Rigidbody2D body;
	private readonly Vector2 rightTunnel = new Vector2 (27.0f, 16.0f);
	private readonly Vector2 leftTunnel  = new Vector2 ( 0.0f, 16.0f);
	
	protected Collider2D coll;
	protected Animator anim;
	protected bool isGamePaused;
	protected bool canMove;
	protected bool frightend;
	protected float speed;
	protected Coroutine moving;
	protected AudioSource source;
	protected Vector2 startPos;
	
	public const float speedValue = 10.0f;
	public LayerMask mask;

	protected virtual void Start () {
		body = GetComponent<Rigidbody2D> ();
		coll = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();
		isGamePaused = false;
		canMove = true;
		frightend = false;
		speed = setSpeed (GameManager.gameManager.level);
		source = GetComponent<AudioSource> ();
		startPos = (Vector2)transform.position;
		GameManager.gameManager.GamePaused += onGamePaused;
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
			moving = StartCoroutine(smoothMove(end));
			return true;
		}
		return false;
	}
	
	protected IEnumerator smoothMove(Vector2 direction) {
		canMove = false;
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
		canMove = (isGamePaused ? false : true);
	}

	private void setAnimation(Vector2 end) {
		Vector2 startPos = (Vector2)transform.position;

		if (frightend) {
			return;
		}

		if (startPos.x - end.x > 0.0f) {
			anim.SetTrigger ("Left");
		} else if (startPos.x - end.x < 0.0f) {
			anim.SetTrigger ("Right");
		} else if (startPos.y - end.y > 0.0f) {
			anim.SetTrigger ("Down");
		} else if (startPos.y - end.y < 0.0f) {
			anim.SetTrigger ("Up");
		}
	}

	protected void onGamePaused() {
		if (isGamePaused) {
			canMove = true;
		}
		isGamePaused = !isGamePaused;
	}

	protected abstract float setSpeed(int level);
}
