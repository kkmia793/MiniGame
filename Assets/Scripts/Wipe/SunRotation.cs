using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SunRotation : MonoBehaviour
{
    [SerializeField]
    Transform _sunTransform;

    [SerializeField]
    float TWEEN_TIME = 25f;

    Vector3 _rotateVector = new Vector3(0, 0, -360);

    // Start is called before the first frame update
    void Start()
    {
        _sunTransform.DORotate(_rotateVector, TWEEN_TIME, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

}
