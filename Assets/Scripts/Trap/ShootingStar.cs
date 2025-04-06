using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using KanKikuchi.AudioManager;

public class ShootingStar : MonoBehaviour,ITrapable
{
    private bool _inCollision = false;
    private Transform _player;
    private Life _playerLife;
    
    private Vector2 _startPos;
    private Vector2 _endPos;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(ConstValues.EARTH_TAG)) return;
        _inCollision = true;
        _player = collision.transform;
        _playerLife = _player.gameObject.GetComponent<Life>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(ConstValues.EARTH_TAG)) return;
        _inCollision = false;
        _player = null;
        _playerLife = null;
    }

    public void Init()
    {
        _startPos = new Vector2(Random.Range(-10, 11), 6);
        _endPos = new Vector2(Random.Range(-10, 11), -6);
        
        transform.parent.position = _startPos;
        transform.parent.DOMove(_endPos, 5f).SetEase(Ease.Linear);
        transform.parent.DOLocalRotate(new Vector3(0 , 0 , 3), .1f).SetLoops(-1, LoopType.Yoyo);
        SEManager.Instance.Play(SEPath.SHOOTING_STAR);
        
        // 画像の向きを定める
        if (_startPos.x < _endPos.x)
        {
            transform.parent.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    public void DestroyObject()
    {
        DOTween.Kill(this.transform.root);
        Destroy(transform.parent.gameObject);
    }

    private void Update()
    {
        if (_inCollision)
        {
            float tmp = Time.deltaTime / ((_player.position - transform.parent.position).magnitude * 2f);   // ゲージが溜まりすぎるから補正をかける意味での2f
            
            _playerLife?.ChargeRecoverGage(tmp);
        }

        if (transform.parent.position == (Vector3)_endPos)
        {
            Invoke(nameof(DestroyObject), 1f);
        }
    }
}
