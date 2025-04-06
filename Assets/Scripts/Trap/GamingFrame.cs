using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GamingFrame : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private bool _isFever = true;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (_isFever)
        {
            spriteRenderer.color = Color.HSVToRGB(Time.time % 1, .3f, .9f);
        }
    }

    public void FadeOut()
    {
        _isFever = false;
        spriteRenderer.DOFade(0, 0.5f);
        Invoke(nameof(Destroy), 0.5f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
