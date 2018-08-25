using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeEffect : MonoBehaviour
{
    [SerializeField] private float _fadeUpdateDelta;
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;
        
    private CanvasGroup _canvasGroup;
    
    
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
        
        var fadeDelta = _fadeUpdateDelta / _fadeInDuration;
        var fadeCount = (int) (1 / fadeDelta);
        
        while (fadeCount-- > 0)
        {
            _canvasGroup.alpha += fadeDelta;
            await new WaitForSeconds(_fadeUpdateDelta);
        }

        _canvasGroup.alpha = 1f;
    }
    
    private async void FadeOutTask()
    {
        _canvasGroup.alpha = 1f;
        
        var fadeDelta = _fadeUpdateDelta / _fadeOutDuration;
        var fadeCount = (int) (1 / fadeDelta);
        
        while (fadeCount-- > 0)
        {
            _canvasGroup.alpha -= fadeDelta;
            await new WaitForSeconds(_fadeUpdateDelta);
        }

        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

}