using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeEffect : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;

    
    public void FadeIn()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, _fadeInDuration).OnStart(() => gameObject.SetActive(true));
    }

    public void FadeOut()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.DOFade(0f, _fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}