using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TanabataZone : MonoBehaviour
{
    public Transform hikobosi; 
    public Transform orihime; 
    public float attractionSpeed = 1f; // 2つのオブジェクトの距離が縮まる速度
    public float scoreIncreaseRate = 10f; // ゾーン内でのスコア増加率

    private bool _isInZone = false;
    private float _zoneScore = 0f;
    private float _zoneMaxScore = 100f;
    
    void Update()
    {
        if (_isInZone)
        {
            UpdateZoneEffect();
            MoveObjectsCloser();
        }
        
    }

    private void UpdateZoneEffect()
    {
        IncreaseScore();
        Debug.Log("Score: " + _zoneScore);
    }

    private void IncreaseScore()
    {
        _zoneScore += scoreIncreaseRate * Time.deltaTime;
        if (_zoneScore >= _zoneMaxScore)
        {
            /*　Scene移動 or リザルトパネル表示*/
        }
    }

    private void DecreaseScore(float amount)
    {
        _zoneScore = Mathf.Max(0, _zoneScore - amount);
    }

    private void MoveObjectsCloser()
    {
        if (hikobosi != null && orihime != null)
        {
            // 2つのオブジェクトをお互いに引き寄せる
            Vector3 direction = (orihime.position - hikobosi.position).normalized;
            float moveStep = attractionSpeed * (_zoneScore / 100f) * Time.deltaTime;

            hikobosi.position += direction * moveStep;
            orihime.position -= direction * moveStep; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTargetZone(other))
        {
            EnterZone();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTargetZone(other))
        {
            ExitZone();
        }
    }

    private bool IsTargetZone(Collider2D other)
    {
        return other.CompareTag("77Zone");
    }

    private void EnterZone()
    {
        _isInZone = true;
    }

    private void ExitZone()
    {
        _isInZone = false;
    }
}
