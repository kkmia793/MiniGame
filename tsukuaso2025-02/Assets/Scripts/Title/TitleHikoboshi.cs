using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleHikoboshi : MonoBehaviour
{
    public RectTransform hikoboshi;  
    public float moveDistance = 10f; 
    public float duration = 1f; 

    void Start()
    {
        hikoboshi.DOAnchorPosX(hikoboshi.anchoredPosition.x + moveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
}
