using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using KanKikuchi.AudioManager;

public class Trap : MonoBehaviour
{
    [SerializeField] private const int Damage = 1;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstValues.EARTH_TAG))
        {
            var life = collision.GetComponent<Life>();

            if (life != null)
            {
                if (!life.isFever)
                {
                    life.DecrementLifeValue(Damage);
                }
                else
                {
                    SEManager.Instance.Play(SEPath.BREAK, 2f);
                    KillDOTween();
                    Destroy(transform.root.gameObject);
                }
            }
        }
        
        if (collision.CompareTag(ConstValues.SUN_TAG))
        {
            SEManager.Instance.Play(SEPath.COLLIDE_SUN);
            KillDOTween();
            Destroy(transform.root.gameObject);
        }
    }

    private void KillDOTween()
    {
        DOTween.Kill(this.transform.root);
    }
}
