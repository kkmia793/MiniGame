using UnityEngine;
using System;

public class earthOrbit : MonoBehaviour
{
    public GameObject sun;

    public float a = 4f;  // 長軸（X方向の半径）
    public float b = 4f;  // 短軸（Y方向の半径）
    
    public float angularSpeed = 1f; 
    public float minSpeed = 0.1f;
    public float maxSpeed = 8f;
    public float aspeedStep = 1f;
    public float bspeedStep = 3f;

    private float _angle = 0f; 
    private Vector3 _previousMousePosition;
    private bool _mouseMoved;
    
    private bool _hasCrossed = false;  
    public  event Action OnCrossLine;

    void Start()
    {
        Life.OnFever += ChangeMaxSpeed;
        Life.OnLifeDecreased += DecreaseSpeed;
    }

    void OnDestroy()
    {
        Life.OnFever -= ChangeMaxSpeed;
        Life.OnLifeDecreased -= DecreaseSpeed;
    }
    
    void Update()
    {
        HandleInput();
        // HandleMouseRotation();
        UpdatePosition();
        CheckCrossingLine(); 
    }
    
    void HandleInput()
    { 
        /* ボタンを押すと加速 */
        if (Input.GetKey(KeyCode.Space))
        {
            angularSpeed = Mathf.Min(angularSpeed + bspeedStep * Time.deltaTime, maxSpeed);
        }
        else
        {
            /* 常に減速 */
            angularSpeed = Mathf.Max(angularSpeed - aspeedStep * Time.deltaTime, minSpeed);
        }
    }
    
    /* ポケモンレンジャー 
    void HandleMouseRotation()
    {
        Vector3 mouseDelta = Input.mousePosition - _previousMousePosition;
        _mouseMoved = mouseDelta.magnitude > 0.1f;

        if (_mouseMoved)
        {
            Vector3 toCenter = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
            float mouseRotationDirection = Mathf.Sign(Vector3.Cross(toCenter, mouseDelta).z);

            if (mouseRotationDirection < 0)
            {
                angularSpeed = Mathf.Min(angularSpeed + speedStep * Time.deltaTime, maxSpeed);
            }
            else if (mouseRotationDirection > 0)
            {
                angularSpeed = Mathf.Max(angularSpeed - speedStep * Time.deltaTime, minSpeed);
            }
        }
        
        _previousMousePosition = Input.mousePosition;
    }
    
    */
    
    void UpdatePosition()
    {
        if (sun == null) return;
        
        Vector2 center = sun.transform.position; 

        _angle += angularSpeed * Time.deltaTime;
        float x = center.x + a * Mathf.Cos(_angle);
        float y = center.y + b * Mathf.Sin(_angle);
        transform.position = new Vector2(x, y);
    }

    // 七夕ラインカウント用
    void CheckCrossingLine()
    {
        if (sun == null) return;

        Vector3 sunPosition = sun.transform.position;
        Vector3 earthPosition = transform.position;
        
        Vector3 upVector = Vector3.up;
        Vector3 direction = earthPosition - sunPosition;
        
        float dotProduct = Vector3.Dot(direction.normalized, upVector);
        float threshold = 0.95f;

        if (!_hasCrossed && dotProduct > threshold)
        {
            _hasCrossed = true;
            
            OnCrossLine?.Invoke();
        }

        if (_hasCrossed && dotProduct < threshold - 0.1f)
        {
            _hasCrossed = false;
        }
    }

    void ChangeMaxSpeed(bool value)
    {
        if (value)
        {
            maxSpeed = maxSpeed * 1.5f;
        }
        else
        {
            maxSpeed = maxSpeed / 1.5f;
        }
    }

    void DecreaseSpeed(int life)
    {
        angularSpeed = minSpeed; 
        
        // angularSpeed = Mathf.Max(angularSpeed / 3f, minSpeed); 
    }
}