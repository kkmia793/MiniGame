using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFaceChanger : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer _earthSR;

    [SerializeField]
    SpriteRenderer _faceSR;

    [SerializeField]
    Sprite[] _earthArray;

    [SerializeField]
    Sprite[] _faceArray;

    private void OnEnable()
    {
        Life.OnLifeDecreased += OnHit;
    }

    private void OnDisable()
    {
        Life.OnLifeDecreased -= OnHit;
    }

    void OnHit(int life)
    {
        if (life <= 0) return;

        int stage = ConstValues.LIFE - life;

        _earthSR.sprite = _earthArray[stage];
        _faceSR.sprite = _faceArray[stage];

    }
}
