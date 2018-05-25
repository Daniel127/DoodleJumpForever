using System.Collections;
using System.Collections.Generic;
using Managers;
using Patrones;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _audioSource;

    public AudioClip BulletClip;
    public AudioClip MonsterKillClip;
    public AudioClip MonsterJumpClip;
    public AudioClip FallingClip;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public void Bullet()
    {
        _audioSource.PlayOneShot(BulletClip);
    }

    public void MonsterKill()
    {
        _audioSource.PlayOneShot(MonsterKillClip);
    }

    public void MonsterJump()
    {
        _audioSource.PlayOneShot(MonsterJumpClip);
    }

    public void Falling()
    {
        _audioSource.PlayOneShot(FallingClip);
    }
}
