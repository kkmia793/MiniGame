using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Blackhole_demo : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 scale;
    void Start()
    {
        scale = transform.localScale;
        Init();
    }
    void Init()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 1f).SetEase(Ease.InCirc);
        this.transform.DORotate(new Vector3(0, 0, 0), 0);
        
        this.transform.DORotate(new Vector3(0, 0, 90),  5).SetEase(Ease.Linear);
        
        Invoke(nameof(Delete), 5);
    }

    private void Delete()
    {
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InCirc);
        Invoke(nameof(Init), 2f);
    }
    
}
