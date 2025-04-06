using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    [SerializeField] private DebrisPool debrisPool;
    [SerializeField] private GameObject shootingStar;
    [SerializeField] private Blackhole blackhole;
    [SerializeField] private WaveMotionGun waveMotionGun;
    [SerializeField] private Explosion explosion;
    
    
    private float _Dtime;
    private float _Stime;
    private float _Btime;
    private float _Wtime;
    private float _Etime;
    
    private void Update()
    {
        _Dtime += Time.deltaTime;
        _Stime += Time.deltaTime;
        _Etime += Time.deltaTime;
        _Btime += Time.deltaTime;
        _Wtime += Time.deltaTime;
        
        if (_Dtime > 3f)
        {
            _Dtime = 0;
            Debris debris = debrisPool.GetPooledObject();
            debris.Init();
        }

        if (_Stime > 5f)
        {
            _Stime = 0;
            var s = Instantiate(shootingStar);
            s.GetComponentInChildren<ShootingStar>().Init();
        }
        
        if (_Etime > 8f)
        {
            _Etime = 0;
            var e = Instantiate(explosion);
            e.Init();
        }
        
        if (_Btime > 10f)
        {
            _Btime = 0;
            var b = Instantiate(blackhole);
            b.Init();
        }
        
        if (_Wtime > 20f)
        {
            _Wtime = 0;
            var w = Instantiate(waveMotionGun);
            w.Init();
        }
    }
}
