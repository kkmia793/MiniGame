using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExplosionWarning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(transform.localScale * 1.2f, .25f).SetLoops(4, LoopType.Yoyo);
        Invoke(nameof(Destroy), 1f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
    
}
