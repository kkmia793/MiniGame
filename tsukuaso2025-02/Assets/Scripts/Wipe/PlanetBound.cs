using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlanetBound : MonoBehaviour
{
    [SerializeField]
    Transform _transform;

    [SerializeField]
    Vector2 _boundDownVector;

    static readonly float BOUND_DOWN_PERCENTAGE = 0.16f;
    public static readonly float TEMPO = 60f / 71.45f;

    void Start()
    {
        DOTween.Sequence()
             .Append(_transform.DOLocalMove(_boundDownVector, TEMPO * BOUND_DOWN_PERCENTAGE).SetEase(Ease.InExpo)).SetRelative(true)
             .Append(_transform.DOLocalMove(-_boundDownVector, TEMPO * (1 - BOUND_DOWN_PERCENTAGE)).SetEase(Ease.OutQuint)).SetRelative(true)
             .SetLoops(-1);
    }
}
