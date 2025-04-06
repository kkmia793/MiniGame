using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleOrihime : MonoBehaviour
{
    public RectTransform orihime;  
    public float moveDistance = 10f; 
    public float duration = 1f; 

    void Start()
    {
        orihime.DOAnchorPosX(orihime.anchoredPosition.x - moveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
}
