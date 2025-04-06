using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunMovement : MonoBehaviour
{
    public float moveSpeed = 2f; 
    private Rigidbody2D _rb;

    private float maxX = 9;
    private float maxY = 6;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); 
    }

    void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    { 
         float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
         float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
    
         float x = Mathf.Clamp(_rb.position.x + moveX, -maxX, maxX);
         float y = Mathf.Clamp(_rb.position.y + moveY, -maxY, maxY);
         
         transform.position += new Vector3(x - _rb.position.x, y - _rb.position.y, 0);
    }
        
}
