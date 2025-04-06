using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class FeverBamboo : MonoBehaviour
{
    static readonly float BOUND_DOWN_PERCENTAGE = 0.16f;
    static readonly float BOUND_LIMIT_SCALE = 0.88f;
    static readonly float TEMPO = 60f / 71.45f;

    private Vector3 defPos;
    RectTransform rt;
    private Sequence sequence;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        defPos = transform.position;

        Life.OnFever += Fade;
    }

    private void OnDestroy()
    {
        Life.OnFever -= Fade;
    }

    private void Fade(bool value)
    {
        if (value)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        transform.DOMove(new Vector3(Mathf.Sign(defPos.x) * (Mathf.Abs(defPos.x) - 5f), transform.position.y, 0), 0.5f);
        
        sequence = DOTween.Sequence()
            .Append(transform.DOLocalMove(new Vector3(Mathf.Sign(defPos.x) * -0.3f, 0.4f, 0f), TEMPO * BOUND_DOWN_PERCENTAGE / 2).SetEase(Ease.InExpo)).SetRelative(true)
            .Append(transform.DOLocalMove(-new Vector3(Mathf.Sign(defPos.x) * -0.3f, 0.4f, 0f), TEMPO * (1 - BOUND_DOWN_PERCENTAGE) / 2).SetEase(Ease.OutQuint)).SetRelative(true)
            .SetLoops(-1)
            .SetDelay(0.5f);
        sequence.Play();
    }

    private void FadeOut()
    {
        transform.DOMove(defPos, 0.5f);
        sequence.Kill();
    }
    
}
