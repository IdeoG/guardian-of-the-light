using DG.Tweening;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.Vignette
{
    public class Vignette : MonoBehaviour
    {
        [SerializeField] private float _maxScale = 200f;
        [SerializeField] private float _minScale = 1f;
        [SerializeField] private float _duration = 5f;
        
        private RectTransform _transform;
        
        public Tweener Collapse()
        {
            _transform.localScale = Vector3.one * _maxScale;
            return _transform.DOScale(_minScale, _duration);
        }

        public Tweener Expand()
        {
            _transform.localScale = Vector3.one * _minScale;
            return _transform.DOScale(_maxScale, _duration);
        }

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

    }
}