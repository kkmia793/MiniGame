using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnRetryPressed);
    }

    private void OnRetryPressed()
    {
        if (SceneController.Instance != null)
        {
            SceneController.Instance.ReloadScene(); 
        }
    }
}