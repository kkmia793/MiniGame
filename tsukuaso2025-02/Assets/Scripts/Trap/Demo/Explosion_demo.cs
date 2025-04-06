using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;

public class Explosion_demo : MonoBehaviour
{
    
    [SerializeField] private ExplosionWarning ew;
    private const float LifeTime = 3f;

    private Vector2 pos;
    private Vector3 scale;
    
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        Init();
    }
    
    public void Init()
    {
        this.GetComponent<SpriteRenderer>().DOFade(endValue: 1f, duration: 0);
        transform.localScale = Vector3.zero;
        Instantiate(ew, transform.position, Quaternion.identity);
        Invoke(nameof(Explode), 1f);
    }

    public void DestroyObject()
    {
        Init();
    }

    private void Explode()
    {
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
    }
}
