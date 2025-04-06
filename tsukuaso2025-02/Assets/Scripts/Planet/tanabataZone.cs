using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tanabataZone : MonoBehaviour
{
    private bool _isInZone = false;
    
    void Update()
    {
        if (_isInZone)
        {
            /*七夕ゾーンにいる間の処理*/
        }              
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("77Zone"))
        {
            _isInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("77Zone"))
        {
            _isInZone = false;
        }
    }
}
