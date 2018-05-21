using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Platform : MonoBehaviour {

	public float JumpForce = 1f;
	public PlatformType type;

	private Animator _animator;
	private AudioSource _audioSource;
	private int direction = 1;

	void Start()
	{
		if (type == PlatformType.Blanca || type == PlatformType.Marron) {
			_animator = GetComponent<Animator> ();
		}
		Init ();
	    _audioSource = GetComponent<AudioSource>();
		Debug.Log ("#Plataforma# collider: " + GetComponent<Collider2D>().isActiveAndEnabled);
	}

	void Update()
	{
		if (type == PlatformType.Azul) {
			if (transform.position.x > 2.2f) {
				direction = -1;
			}
			else if (transform.position.x < -2.2f) {
				direction = 1;
			}
			transform.Translate(Vector3.right * direction * 1.75f * Time.deltaTime);
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
		}

		if (type == PlatformType.Blanca) {
			_animator.SetTrigger ("Destroy");
		}

        _audioSource.Play();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (type == PlatformType.Marron && collider.transform.position.y > transform.position.y) {
			_animator.SetTrigger ("Destroy");
			GetComponent<Collider2D>().enabled = false;
			_audioSource.Play ();
			Debug.Log ("#Plataforma# collider: " + GetComponent<Collider2D>().isActiveAndEnabled);
		}
	}

	public void Init()
	{
		//_animator?.SetTrigger ("Idle");
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