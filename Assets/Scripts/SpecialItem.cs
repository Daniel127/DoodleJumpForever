using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour {
    public float JumpForce = 1f;
    private AudioSource _audioSource;
    private Animator _animator;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        if (collision.relativeVelocity.y > 0f || (collision.transform.position.y + collision.collider.offset.y) < transform.position.y) return;

        collision.gameObject.GetComponent<Player>().Impulse(JumpForce);
        _animator.SetTrigger(gameObject.tag.Contains("Trampoline") ? "Jumping" : "Spring");
        _audioSource.Play();
    }
}
