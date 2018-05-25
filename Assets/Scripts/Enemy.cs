using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class Enemy : MonoBehaviour
{
	[Header("Collider Cabeza")]
	public Vector2 Origin;
	public Vector2 Size;

	private void Update()
	{
		Vector2 origin = transform.position;
		origin += Origin;
		RaycastHit2D hit = Physics2D.BoxCast(origin, Size, 0, Vector2.zero, 1);
		if (!hit.transform || !hit.transform.CompareTag("Player")) return;
		hit.transform.gameObject.GetComponent<Player>().Impulse(10f);
		Delete(true);
	}

	public void Delete(bool onJump = false)
	{
	    if (onJump)
	    {
	        SoundManager.Instance.MonsterJump();
	    }
        LevelManager.Instance.RemoveLevelObject(gameObject.transform.parent.gameObject);
	}
}