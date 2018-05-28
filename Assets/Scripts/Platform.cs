using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

public class Platform : MonoBehaviour {

	public float JumpForce = 1f;
	public PlatformType type;
    
    private Animator _animator;
	private AudioSource _audioSource;
	private int _direction = 1;
    private float _specialItem = 0.05f;
    private float _trampoline = 0.2f;

    void Start()
	{
		if (type == PlatformType.Blanca || type == PlatformType.Marron) {
			_animator = GetComponent<Animator> ();
		}
		Init ();
	    _audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (type == PlatformType.Azul) {
			if (transform.position.x > 2.2f) {
				_direction = -1;
			}
			else if (transform.position.x < -2.2f) {
				_direction = 1;
			}
			transform.Translate(Vector3.right * _direction * 1.75f * Time.deltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.CompareTag("Player")) return;
		if (collision.relativeVelocity.y > 0f || (collision.transform.position.y + collision.collider.offset.y) < transform.position.y) return;

		if (type != PlatformType.Marron) {
			collision.gameObject.GetComponent<Player> ().Impulse (JumpForce);
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
		}
	}

	public void Init()
	{
		//_animator?.SetTrigger ("Idle");
		GetComponent<Collider2D> ().enabled = true;

        if (type == PlatformType.Verde)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
            }

            if (Random.Range(0f, 1f) <= _specialItem)
            {
                if (Random.Range(0f, 1f) <= _trampoline)
                {
                    ActivateItem(Random.Range(0f, 1f) < 0.5f ? 0 : 1);
                }
                else
                {
                    ActivateItem(Random.Range(0f, 1f) < 0.5f ? 2 : 3);
                }
            }
        }
    }

    private void ActivateItem(int idx)
    {
        var child = transform.GetChild(idx);
        if (!child.gameObject.activeSelf)
        {
            child.gameObject.SetActive(true);
        }
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