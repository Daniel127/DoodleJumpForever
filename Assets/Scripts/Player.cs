using Managers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	private SpriteRenderer _sprite;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private float _movement;
	private float _timeShooting;
	private float _endShoot;
	private BoxCollider2D _collider;

	[Header("Movement")]
	public float Velocity = 1f;

	[Header("Shooter")]
	public GameObject Bullet;
	public Transform Barrel;
	[Range(0.1f, 0.4f)]
	public float TimeShooting = 0.3f;
	[Range(0.1f, 1)]
	public float IntervalTime;

	private void Start ()
	{
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();
		_collider = GetComponent<BoxCollider2D> ();
	}

	private void Update ()
	{
		_movement = Input.GetAxis("Horizontal") * Velocity;
		if (_movement < 0)
			_sprite.flipX = true;
		else if (_movement > 0)
			_sprite.flipX = false;

		if (Input.GetButtonDown("Fire")) //Configurado con las teclas correctas en el editor
		{
			_endShoot = Time.time + TimeShooting;
			if (_timeShooting <= Time.time)
			{
				_timeShooting = Time.time + IntervalTime;
				Shoot();
			}
		}
		else if(_endShoot <= Time.time)
		{
			_animator.SetBool("Shooting", false);
		}

		_animator.SetFloat("VelocityY", _rigidbody.velocity.y);
	}

	public void Init()
	{
		if(_collider)
			_collider.enabled = true;
	}

	private void Shoot()
	{
		GameObject bulletObject = PoolManager.Instance.Spawn(Bullet, Barrel.position, Quaternion.identity);
		Bullet bullet = bulletObject.GetComponent<Bullet>();
		bullet.InitDestroy();

		_animator.SetBool("Shooting",true);
	}

	private void FixedUpdate()
	{
		Vector2 newVelocity = _rigidbody.velocity;
		newVelocity.x = _movement;
		_rigidbody.velocity = newVelocity;
	}

	private void LateUpdate()
	{
		if (!_sprite.isVisible)
			OnBecameInvisible();
	}

	public void OnBecameInvisible()
	{
		Vector3 newPosition = transform.position;
		if (transform.position.x < -2.5)
			newPosition.x = 2.5f;
		else if (transform.position.x > 2.5)
			newPosition.x = -2.5f;
		transform.position = newPosition;

		if (!(transform.position.y < -5)) return;
		_animator.SetTrigger ("Jump");
	    if (!SoundManager.Instance.IsPlaying())
	    {
	        SoundManager.Instance.Falling();
	    }
		GameManager.Instance.EndGame = true;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag("Enemy")) return;
		Debug.Log ("#Player# chocaConEnemigo");
		_animator.SetTrigger ("Jump");
	    SoundManager.Instance.Falling();
        _collider.enabled = false;
	}

	public void Impulse(float jumpForce)
	{
		Vector2 velocity = _rigidbody.velocity;
		velocity.y = jumpForce;
		_rigidbody.velocity = velocity;
	}
}