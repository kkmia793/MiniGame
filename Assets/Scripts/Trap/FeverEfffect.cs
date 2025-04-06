using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class FeverEfffect : MonoBehaviour
{
    [SerializeField] private GamingFrame frame;
    private GamingFrame _frame;
    
    [SerializeField] private FeverBamboo[] feverBamboo;
    
    private ParticleSystem _ps;
    private void OnEnable()
    {
        Life.OnFever += SwitchState;
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        Life.OnFever -= SwitchState;
    }

    private void SwitchState(bool state)
    {
        if (state)
        {
            _ps.Play();
            _frame = Instantiate(frame);
            
            BGMManager.Instance.ChangeBaseVolume(0f);
            SEManager.Instance.Play(SEPath.LIGHTS_OUT_X2);
            SEManager.Instance.Play(SEPath.FEVER_VOICE);
        }
        else
        {
            _ps.Stop();
            _frame.FadeOut();
            
            SEManager.Instance.Stop(SEPath.LIGHTS_OUT_X2);
            BGMManager.Instance.ChangeBaseVolume(1f);
        }
    }
}
