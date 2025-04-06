using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DebrisPool : MonoBehaviour
{
    [SerializeField] private const int InitPoolSize = 10;
    [SerializeField] private Debris debris;
    
    // プールされたオブジェクトをコレクションに格納する
    private Stack<Debris> stack;
    
    private void Start()
    {
        SetupPool();
    }
    
    // プールを作成する（ラグが目立たなくなったら呼び出す）
    private void SetupPool()
    {
        stack = new Stack<Debris>();

        for (int i = 0; i < InitPoolSize; i++)
        {
            var instance = Instantiate(debris);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }
    
    // プールから最初のアクティブなGameObjectを返す
    public Debris GetPooledObject()
    {
        // プールが十分に大きくない場合、新しいPooledObjectをインスタンス化する
        if (stack.Count == 0)
        {
            Debris newInstance = Instantiate(debris);
            newInstance.Pool = this;
            return newInstance;
        }
        // それ以外の場合は、単にリストから次のものを取得する
        Debris nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }
    
    public void ReturnToPool(Debris pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
