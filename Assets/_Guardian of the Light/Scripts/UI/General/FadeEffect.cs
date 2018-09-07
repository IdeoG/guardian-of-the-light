using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeEffect : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;
    private const float _fadeWaitDelayPeriod = 0.04f;


    public void FadeIn()
    {
        FadeInTask();
    }

    public void FadeOut()
    {
        FadeOutTask();
    }

    private async void FadeInTask()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;

        var fadeDelta = _fadeWaitDelayPeriod / _fadeInDuration;
        var fadeCount = (int) (1 / fadeDelta);

        while (fadeCount-- > 0)
        {
            _canvasGroup.alpha += fadeDelta;
            await new WaitForSeconds(_fadeWaitDelayPeriod);
        }

        _canvasGroup.alpha = 1f;
    }

    private async void FadeOutTask()
    {
        _canvasGroup.alpha = 1f;

        var fadeDelta = _fadeWaitDelayPeriod / _fadeOutDuration;
        var fadeCount = (int) (1 / fadeDelta);

        while (fadeCount-- > 0)
        {
            _canvasGroup.alpha -= fadeDelta;
            await new WaitForSeconds(_fadeWaitDelayPeriod);
        }

        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}