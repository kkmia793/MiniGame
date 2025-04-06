using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using KanKikuchi.AudioManager;

public class Explosion : Trap, ITrapable
{
    [SerializeField] private ExplosionWarning ew;
    private const float LifeTime = 3f;

    private Vector2 pos;
    private Vector3 scale;
    
    public void Init()
    {
        scale = transform.localScale;
        transform.localScale = Vector3.zero;
        pos = new Vector2(Random.Range(-8, 9), Random.Range(-4, 5));
        Instantiate(ew, pos, Quaternion.identity);
        Invoke(nameof(Explode), 1f);
    }
    
    public void DestroyObject()
    {
        DOTween.Kill(this.transform);
        Destroy(gameObject);
    }

    private void Explode()
    {
        transform.position = pos;
        transform.DOScale(scale * 3f, LifeTime).SetEase(Ease.InCirc);
        transform.DORotate(new Vector3(0, 0, Random.Range(-45, 45)), LifeTime).SetEase(Ease.Linear);
        Invoke(nameof(CollisionDestroy),LifeTime - 0.5f);
        
        //1秒でImageのアルファを0にする
        this.GetComponent<SpriteRenderer>().DOFade(endValue: 0f, duration: .5f).SetDelay(LifeTime - 0.5f);
        Invoke(nameof(DestroyObject),LifeTime + 1f);
    }

    private void CollisionDestroy()
    {
        GetComponent<Collider2D>().enabled = false;
        SEManager.Instance.Play(SEPath.EXPLOSION);
    }
}
