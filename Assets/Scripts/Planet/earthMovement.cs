using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMovement : MonoBehaviour
{
    public GameObject sun;  // 太陽（重力の中心）
    public float initVelocityX = 0f; // 初速度X方向
    public float initVelocityY = 3.65f; // 初速度Y方向
    public float G = 6.67E-5f; // 万有引力定数（2D版の調整値）
    public float forceMultiplier = 2f; // 引力強化倍率
    public float boostedForceMultiplier = 5f; // 左クリック時の引力強化倍率
    public float minDistance = 0.5f; // GameOver判定の距離

    public static event System.Action OnGameOver; // GameOverイベント

    private Rigidbody2D rb;
    private float m; // 惑星の質量
    private float M; // 太陽の質量

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 

        // 初速度を与える
        rb.velocity = new Vector2(initVelocityX, initVelocityY);

        // 質量を取得
        m = rb.mass;
        M = sun.GetComponent<Rigidbody2D>().mass;
    }

    void FixedUpdate()
    {
        // 太陽と惑星の距離を計算
        Vector2 direction = sun.transform.position - transform.position;
        float r = direction.magnitude;

        // GameOver判定
        if (r <= minDistance)
        {
            GameOver();
            return;
        }

        // 万有引力の計算（F = G * (m * M) / r^2）
        float forceMultiplierToUse = Input.GetMouseButton(0) ? boostedForceMultiplier : forceMultiplier;
        float forceMagnitude = G * m * M / (r * r) * forceMultiplierToUse;
        Vector2 force = direction.normalized * forceMagnitude;

        // 惑星に万有引力を適用
        rb.AddForce(force, ForceMode2D.Force);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        OnGameOver?.Invoke(); 
    }
}