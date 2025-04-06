using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WipeScript : MonoBehaviour
{
    public enum Face
    {
        Normal,
        Fever,
        Shocked
    }

    [SerializeField]
    Image _image;

    [SerializeField]
    RectTransform rt;

    [SerializeField]
    Sprite[] _faceType;

    Sequence boundSeq;
    private Sequence feverSeq;
    Tween tween;

    static readonly float BOUND_DOWN_PERCENTAGE = 0.16f;
    static readonly float BOUND_LIMIT_SCALE = 0.88f;
    static readonly float TEMPO = 60f / 71.45f;
    static readonly float FREEZE_TIME = 0.8f;
    static readonly float SHOCK_TIME = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Life.OnLifeDecreased += OnHit;
        Life.OnFever += OnFeverTime;

        boundSeq = DOTween.Sequence()
            .Append(rt.DOScaleY(BOUND_LIMIT_SCALE, TEMPO * BOUND_DOWN_PERCENTAGE).SetEase(Ease.InExpo))
            .Append(rt.DOScaleY(1, TEMPO * (1 - BOUND_DOWN_PERCENTAGE)).SetEase(Ease.OutQuint))
            .SetLoops(-1);

        StartBound();
    }

    void StartBound()
    {
        boundSeq.Play();
    }

    void PauseAndPlay()
    {
        DOTween.Sequence()
            .AppendCallback(() => boundSeq.Pause())
            .AppendInterval(FREEZE_TIME)
            .AppendCallback(StartBound);
    }

    void OnHit(int life)
    {
        ChangeFace(Face.Shocked, SHOCK_TIME);
        PauseAndPlay();
    }

    void OnFeverTime(bool value)
    {
        if (value)
        {
            feverSeq = DOTween.Sequence()
                .Append(rt.DOScaleY(BOUND_LIMIT_SCALE,  TEMPO * BOUND_DOWN_PERCENTAGE / 2).SetEase(Ease.InExpo))
                .Append(rt.DOScaleY(1, TEMPO * (1 - BOUND_DOWN_PERCENTAGE) / 2).SetEase(Ease.OutQuint))
                .SetLoops(-1);
            
            boundSeq.Pause();
            feverSeq.Play();
            ChangeFace(Face.Fever, ConstValues.FEVERTIME);
        }
        else
        {
            Debug.Log("end");
            feverSeq.Kill();
            StartBound();
        }
    }

    void ChangeFace(Face face, float time)
    {
        tween?.Kill();
        SetFaceSprite(face);
        tween = DOVirtual.DelayedCall(time, () => SetFaceSprite(Face.Normal));

        void SetFaceSprite(Face face)
        {
            if(_image == null) return;
            _image.sprite = _faceType[(int)face];
        }
    }
}
