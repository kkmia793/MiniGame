using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Life : MonoBehaviour
{
    private int _life = ConstValues.LIFE;
    [SerializeField]private float recover;
    private const float MaxRecover = 1f;
    
    private const float InvincibleTime = 2f;
    public bool _isInvincible = false;
    private IEnumerator _invincibleRoutine;
    
    public bool isFever = false;
    
    public static event Action<int> OnLifeDecreased;
    public static event Action<float> OnRecoverChanged;
    
    public static event Action<bool> OnFever; // true: フィーバー開始 / false: フィーバー終了

    private void OnEnable()
    {
        _invincibleRoutine = Invincible(0f);    // 初期化
    }

    public void DecrementLifeValue(int value)
    {
        if(_isInvincible) return;
        
        _life -= value;
        StartInvincible(InvincibleTime);
        OnLifeDecreased?.Invoke(_life);
    }

    private void IncrementLifeValue(int value)
    {
        _life += value;
    }

    // 無敵
    private void StartInvincible(float time)
    {
        StopCoroutine(_invincibleRoutine);
        _invincibleRoutine = Invincible(time);
        StartCoroutine(_invincibleRoutine);
    }
    private IEnumerator Invincible(float time)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(time);
        _isInvincible = false;
    }

    // フィーバー
    public void ChargeRecoverGage(float value)
    {
        if (isFever) return;
        
        recover += value;
        OnRecoverChanged?.Invoke(recover / MaxRecover);   // ゲージの溜まり具合

        if (recover >= MaxRecover)
        {
            recover -= MaxRecover;
            //IncrementLifeValue(1);
            StartCoroutine(Fever());
        }
    }

    private IEnumerator Fever()
    {
        isFever = true;
        OnFever?.Invoke(isFever);
        StartInvincible(ConstValues.FEVERTIME);
        yield return new WaitForSeconds(ConstValues.FEVERTIME);
        isFever = false;
        OnFever?.Invoke(isFever);
    }
}
