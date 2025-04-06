using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using KanKikuchi.AudioManager;

public class WaveMotionGun : MonoBehaviour,ITrapable
{
    private CinemachineImpulseSource impulseSource;
    [SerializeField]private EmergencyMark emergencyMark;

    [SerializeField] private Transform waveTransform;
    
    public void Init()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        StartCoroutine(Fire(Random.Range(-4, 5)));
    }

    public void DestroyObject()
    {
        DOTween.Kill(this.transform);
        Destroy(gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstValues.EARTH_TAG))
        {
            collision.GetComponent<Life>()?.DecrementLifeValue(1);  // ダメージ量がマジックナンバー
        }
    }

    
    private IEnumerator Fire(float point)
    {
        // 予備モーション
        transform.position = new Vector3(21, point, 0);
        transform.localScale = Vector3.zero;
        
        Instantiate(emergencyMark, new Vector3(8, point, 0), Quaternion.identity);
        SEManager.Instance.Play(SEPath.EMERGENCY);
        
        yield return new WaitForSeconds(1f);
        
        SEManager.Instance.Play(SEPath.EMERGENCY);
        
        yield return new WaitForSeconds(2f);

        // 発射
        var fireSequence = DOTween.Sequence()
            .Append(this.transform.DOMove(new Vector3(0f, point, 0), .2f))
            .Join(this.transform.DOScale(new Vector3(2, 2, 1), .2f));
        
        fireSequence.Play();
        impulseSource.GenerateImpulse();
        SEManager.Instance.Play(SEPath.WAVE_MOTION_GUN_FIRE, 1.5f);
        
        yield return new WaitForSeconds(.2f);
        this.transform.DOScale(new Vector3(2f, 2.1f, 1), .3f).SetLoops(3, LoopType.Yoyo);
        this.transform.DOMove(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), .2f).SetLoops(25, LoopType.Yoyo);
        this.transform.DOLocalRotate(new Vector3(0, 0, 1), 0.1f).SetLoops(50, LoopType.Yoyo);

        waveTransform.DOLocalMoveX(-5f, 5f).SetEase(Ease.Linear);
        
        yield return new WaitForSeconds(5f);
        
        // 撤収
        var deleteSequence = DOTween.Sequence()
            .Append(this.transform.DOMove(new Vector3(-21f, transform.position.y, 0), .5f))
            .Join(this.transform.DOScale(Vector3.zero, .3f));
        
        deleteSequence.Play();
        
        yield return new WaitForSeconds(1f);
        DestroyObject();
    }
}
