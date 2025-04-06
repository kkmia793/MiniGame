using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text crossCountText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject countdownTextObject; 

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += SetUIVisibility;
            GameManager.Instance.OnCountdownStart += ShowCountdown;
            GameManager.Instance.OnCountdownComplete += HideCountdown;
            GameManager.Instance.OnTimeUpdated += UpdateTimerText;
            GameManager.Instance.OnCrossCountUpdated += UpdateCrossCountText;
            GameManager.Instance.OnGameOver += ShowGameOverScreen;
        }

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryPressed);
        }

        InitializeUI();
    }

    private void InitializeUI()
    {
        if (crossCountText != null) crossCountText.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (retryButton != null) retryButton.gameObject.SetActive(false);
        if (countdownTextObject != null) countdownTextObject.SetActive(false); 
    }

    private void SetUIVisibility(GameManager.GameState state)
    {
        bool isInit = state == GameManager.GameState.Init;
        bool isPlaying = state == GameManager.GameState.Playing;
        bool isGameOver = state == GameManager.GameState.GameOver;

        if (crossCountText != null) crossCountText.gameObject.SetActive(isPlaying);
        if (timerText != null) timerText.gameObject.SetActive(isPlaying);
        if (gameOverPanel != null) gameOverPanel.SetActive(isGameOver);
        if (retryButton != null) retryButton.gameObject.SetActive(isGameOver);

        if (isInit)
        {
            InitializeUI(); 
        }
    }

    private void ShowCountdown()
    {
        if (countdownTextObject != null)
        {
            countdownTextObject.SetActive(true); 
        }
    }

    private void HideCountdown()
    {
        if (countdownTextObject != null)
        {
            countdownTextObject.SetActive(false); 
        }
    }

    public void UpdateCrossCountText(int count)
    {
        if (crossCountText != null)
        {
            crossCountText.text = $"{count}";
        }
    }

    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = $"{time:F0}";
        }
    }

    private void ShowGameOverScreen(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = $"{finalScore}";
        }

        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(true);
        }
        
        SEManager.Instance.Play(SEPath.RESULT);
    }

    private void OnRetryPressed()
    {
        GameManager.Instance?.ResetGame();
    }
}
