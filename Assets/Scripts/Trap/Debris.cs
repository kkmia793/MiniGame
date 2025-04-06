using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debris : Trap, ITrapable
{
    private DebrisPool pool;
    public DebrisPool Pool { get => pool; set => pool = value; }
    
    private Vector2 _startPos;
    private Vector2 _endPos;
    
    public void Init()
    {
        int rand = (Random.Range(0, 2) * 2 - 1) * 10;
        _startPos = new Vector2(rand, Random.Range(-6, 7));
        _endPos = new Vector2(-rand, Random.Range(-6, 7));
        
        float r = 0.6f * Random.Range(1, 4);
        transform.localScale = new Vector3(r * this.transform.localScale.x, r * this.transform.localScale.x, 1);
        
        transform.position = _startPos;
        this.transform.DOMove(_endPos, 10f).SetEase(Ease.Linear);
        this.transform.DORotate(new Vector3(0, 0, Random.Range(90, 270)), 10f).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (transform.position == (Vector3)(_endPos))
        {
            Release();
        }
    }

    public void DestroyObject()
    {
        DOTween.Kill(this.transform);
    }


    private void Release()
    {
        pool.ReturnToPool(this);
    }
}
