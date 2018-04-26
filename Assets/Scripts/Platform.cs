using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	public float JumpForce = 1f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.CompareTag("Player")) return;
		if (collision.relativeVelocity.y > 0f || collision.transform.position.y < transform.position.y) return;

		Rigidbody2D rigidBody = collision.collider.GetComponent<Rigidbody2D>();
		Vector2 velocity = rigidBody.velocity;
		velocity.y = JumpForce;
		rigidBody.velocity = velocity;
	}
}