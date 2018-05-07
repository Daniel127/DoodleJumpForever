using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Platform : MonoBehaviour {

	public float JumpForce = 1f;
	public PlatformType type;

	private Animator _animator;

	void Start()
	{
		if (type == PlatformType.Blanca || type == PlatformType.Marron) {
			_animator = GetComponent<Animator> ();
			//_animator.SetBool ("Idle", true);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.CompareTag("Player")) return;
		if (collision.relativeVelocity.y > 0f || (collision.transform.position.y + collision.collider.offset.y) < transform.position.y) return;

		if (type != PlatformType.Marron) {
			Rigidbody2D rigidBody = collision.collider.GetComponent<Rigidbody2D> ();
			Vector2 velocity = rigidBody.velocity;
			velocity.y = JumpForce;
			rigidBody.velocity = velocity;
		} else {
			GetComponent<Collider2D> ().enabled = false;
		}

		if (type == PlatformType.Blanca || type == PlatformType.Marron) {
			_animator.SetBool ("Destroy", true);
		}
	}

	public void Init()
	{
		_animator?.SetBool ("Idle", true);
		GetComponent<Collider2D> ().enabled = true;
	}

	public void DestroyPlatform() {
		LevelManager.Instance.RemoveLevelObject (gameObject);
	}

	public enum PlatformType {
		Azul,
		Blanca,
		Marron,
		Verde
	}
}