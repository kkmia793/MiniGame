using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title"; 

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnTitlePressed);
    }

    private void OnTitlePressed()
    {
        SceneController.Instance.LoadScene(titleSceneName);
    }
}