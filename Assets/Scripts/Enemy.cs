using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Enemy : MonoBehaviour {

	[Header("Collider Cabeza")]
	public Vector2 Origin;
	public Vector2 Size;

	void Update () {
		Vector2 origin = transform.position;
		origin += Origin;
		RaycastHit2D hit = Physics2D.BoxCast(origin, Size, 0, Vector2.zero, 1);
		if (hit.transform && hit.transform.CompareTag ("Player")) {
			hit.transform.gameObject.GetComponent<Player> ().Impulse (10f);
			Delete ();
		}
	}

	public void Delete()
	{
		PoolManager.Instance.Despawn (gameObject.transform.parent.gameObject);
	}
}
