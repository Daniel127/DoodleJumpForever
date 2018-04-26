using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	private Rigidbody2D _rigidbody;

	public float DestroyTime;
	public float Velocity;

	private void Start()
	{
	}

	public void InitDestroy()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_rigidbody.velocity = new Vector2(0, Velocity);
		StartCoroutine(DespawnInSeconds(DestroyTime));
	}

	public IEnumerator DespawnInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		PoolManager.Instance.Despawn(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		//TODO Hacer
	}
}