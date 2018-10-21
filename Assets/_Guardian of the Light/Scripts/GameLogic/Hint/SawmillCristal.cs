using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class SawmillCristal : UiHint
    {
        [Header("Panel")] 
        [SerializeField] private float dampingTimeSec;
        [Range(0f,1f)] [SerializeField] private float _panelCristalDampingStart;
        [SerializeField] private MeshRenderer _mainCristal;
        [SerializeField] private Light _mainCristalLight;
        [SerializeField] private List<MeshRenderer> _panelCristals;
        
        [Header("Main Cristal Color")] 
        [SerializeField] private float _maxLightIntensity;
        [SerializeField] private AnimationCurve _mainCristalСolorCurve;
        [ColorUsage(false,true)] [SerializeField] private Color _maxMainCristalColor;
        [ColorUsage(false,true)] [SerializeField] private Color _minMainCristalColor;
        
        [Header("Panel Cristal Color")] 
        [SerializeField] private AnimationCurve _panelCristalСolorCurve;
        [ColorUsage(false,true)] [SerializeField] private Color _maxPanelCristalColor;
        [ColorUsage(false,true)] [SerializeField] private Color _minPanelCristalColor;
        
        public override void DestroyItem()
        {
            StartCoroutine(FadePanel());
        }

        private IEnumerator FadePanel()
        {
            var overallTimeMs = 0f;

            while (overallTimeMs < dampingTimeSec * (1 + _panelCristalDampingStart))
            {
                yield return null;

                if (overallTimeMs < dampingTimeSec )
                {
                    var percent = overallTimeMs / dampingTimeSec;
                    _mainCristal.material.SetColor("_EmissionColor", 
                        Color.Lerp(_minMainCristalColor, _maxMainCristalColor, 1 - _mainCristalСolorCurve.Evaluate(percent)));
                    _mainCristalLight.intensity = _maxLightIntensity * (1 - _mainCristalСolorCurve.Evaluate(percent));
                } 
                else if (_mainCristal.gameObject.activeSelf)
                {
                    _mainCristal.gameObject.SetActive(false);
                }

                if (overallTimeMs > dampingTimeSec * _panelCristalDampingStart)
                {
                    var percent = (overallTimeMs - dampingTimeSec * _panelCristalDampingStart ) / dampingTimeSec;
                    
                    foreach (var cristal in _panelCristals)
                    {
                        cristal.material.SetColor("_Color0", 
                            Color.Lerp(_minPanelCristalColor, _maxPanelCristalColor, 1 - _panelCristalСolorCurve.Evaluate(percent)));
                    }
                }
                overallTimeMs += Time.deltaTime;      
            }
            
            Destroy(gameObject);
        }
    }
}