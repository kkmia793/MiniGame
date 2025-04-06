using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultTextAnimation : MonoBehaviour
{
    [SerializeField] private Text finalScoreText;

    private void OnEnable()
    {
        finalScoreTextBounceAnimation();
    }
    
    void finalScoreTextBounceAnimation()
    {
        finalScoreText.transform.localScale = Vector3.one; 
        finalScoreText.transform.DOScale(1.5f, 0.2f)  
            .SetEase(Ease.OutQuad) 
            .OnComplete(() =>
            {
                finalScoreText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
            });
    }
}