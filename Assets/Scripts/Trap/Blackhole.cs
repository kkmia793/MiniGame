using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blackhole : MonoBehaviour, ITrapable
{
    private Transform _player;
    private const int LifeTime = 4;
    private readonly float _power = 1f;

    public void Init()
    {
        var scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 1f).SetEase(Ease.InCirc);
        
        transform.position = new Vector2(Random.Range(-8f, 9f), Random.Range(-4f, 5f));
        this.transform.DORotate(new Vector3(0, 0, 90),  LifeTime + 1).SetEase(Ease.Linear);
        
        Invoke(nameof(Delete), LifeTime);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void Delete()
    {
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InCirc);
        Invoke(nameof(DestroyObject), 2f);
    }
    
    private void FixedUpdate()
    {
        if (_player is null) return;
        var tmp = (transform.position - _player.position).normalized;
        
        if (tmp.magnitude == 0) return;
        _player.position += (tmp * (_power * Time.deltaTime)) / tmp.magnitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstValues.SUN_TAG))
        {
            _player = collision.transform;
        }
    }
}
