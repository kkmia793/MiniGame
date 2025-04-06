using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

using KanKikuchi.AudioManager;
using UnityEngine.Serialization;

public class DamageEffect : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;
    private ParticleSystem _particleSystem;
    [SerializeField]private SpriteRenderer[] spriteRenderer;

    private void OnEnable()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _particleSystem = GetComponent<ParticleSystem>();

        Life.OnLifeDecreased += Damage;
    }

    private void OnDisable()
    {
        Life.OnLifeDecreased -= Damage;
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Damage();
    //     }
    // }

    private void Damage(int i)
    {
        _impulseSource.GenerateImpulse();
        _particleSystem.Play();
        
        SEManager.Instance.Play(SEPath.COLLIDE_EARTH);
        foreach (var renderer in spriteRenderer)
        {
            renderer.DOFade(0, 0.25f).SetLoops(8, LoopType.Yoyo);  // 無敵時間に合わせてるからマジックナンバーだらけ
        }
    }
}
