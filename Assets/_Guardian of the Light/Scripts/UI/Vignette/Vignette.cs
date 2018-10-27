using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.Vignette
{
    public class Vignette : MonoBehaviour, IVignette
    {
        [SerializeField] private float _maxScale = 200f;
        [SerializeField] private float _minScale = 1f;
        [SerializeField] private float _duration = 3f;
        
        private RectTransform _transform;
        
        public IEnumerator Collapse()
        {
            Debug.Log("Collapse 1");
            _transform.localScale = _maxScale * Vector3.one;

            var currentDuration = 0f;
            while (currentDuration < _duration)
            {
                yield return null;

                var scale = (_maxScale * (_duration - currentDuration) + _minScale * currentDuration) / _duration;
                _transform.localScale = Vector3.one * scale; 
                currentDuration += Time.deltaTime;
            }
            
            Debug.Log("Collapse 2");
        }

        public IEnumerator Expand()
        {
            Debug.Log("Expand 1");
            _transform.localScale = _minScale * Vector3.one;

            var currentDuration = 0f;
            while (currentDuration < _duration)
            {
                yield return null;

                var scale = (_minScale * (_duration - currentDuration) + _maxScale * currentDuration) / _duration;
                _transform.localScale = Vector3.one * scale; 
                currentDuration += Time.deltaTime;
            }
            
            Debug.Log("Expand 2");
        }

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            DontDestroyOnLoad(gameObject);
        }
    }
}