using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultPanelAnimation : MonoBehaviour
{
    Tweener tweener = null;


    private void OnEnable()
    {
        AnimateResultPanel();
    }

    public void AnimateResultPanel()
    {
        // 再生中のアニメーションを停止/初期化
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
            transform.localScale = Vector3.one;
        }
        tweener = transform.DOPunchScale(
            punch: Vector3.one * 0.1f,
            duration: 0.2f,
            vibrato: 1
        ).SetEase(Ease.OutExpo);
    }
    
    
}
