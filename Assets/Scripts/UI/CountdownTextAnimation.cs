using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CountdownTextAnimation : MonoBehaviour
{
    [SerializeField] private Text countdownText;

    private void OnEnable()
    {
        StartCoroutine(PlayCountdown()); 
    }

    private IEnumerator PlayCountdown()
    {
        if (countdownText == null) yield break;

        countdownText.text = "Ready？";
        countdownText.transform.localScale = Vector3.one * 0.8f;
        countdownText.color = new Color(1f, 1f, 1f, 1f);

        yield return AnimateText("Ready？", 1.5f, 1.2f);
        yield return AnimateText("GO！", 1.0f, 1.5f);

        gameObject.SetActive(false); 
    }

    private IEnumerator AnimateText(string text, float duration, float scaleMultiplier)
    {
        countdownText.text = text;
        countdownText.transform.localScale = Vector3.one * 0.8f;
        countdownText.color = new Color(1f, 1f, 1f, 1f);

        Sequence seq = DOTween.Sequence();
        seq.Append(countdownText.transform.DOScale(scaleMultiplier, duration / 2).SetEase(Ease.OutBack))
            .Join(countdownText.DOFade(0.5f, duration / 2))
            .AppendInterval(duration / 2)
            .Append(countdownText.DOFade(1f, duration / 2));

        yield return seq.WaitForCompletion();
    }
}