using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmergencyMark : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetLoops(5, LoopType.Yoyo);
        Invoke(nameof(Destroy), 3f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
