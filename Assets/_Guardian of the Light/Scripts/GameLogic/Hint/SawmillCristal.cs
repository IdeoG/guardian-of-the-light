using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class SawmillCristal : UiHint
    {
        [Header("Cristals")]
        [SerializeField] private float _roofDelayTimeSec;
        [SerializeField] private float _roofDampingDurationSec;
        [SerializeField] private float _panelDampingDurationSec;
        [SerializeField] private MeshRenderer _mainCristal;
        [SerializeField] private List<Light> _roofLights;
        [SerializeField] private List<MeshRenderer> _roofCristals;
        [SerializeField] private List<MeshRenderer> _panelCristals;
        
        [Header("Roof Cristal Color")] 
        [SerializeField] private AnimationCurve _roofCristalСolorCurve;
        [SerializeField] private float _maxIntensityRoofLight;
        [ColorUsage(false,true)] [SerializeField] private Color _maxRoofCristalColor;
        [ColorUsage(false,true)] [SerializeField] private Color _minRoofCristalColor;
        
        [Header("Panel Cristal Color")] 
        [SerializeField] private AnimationCurve _panelCristalСolorCurve;
        [ColorUsage(false,true)] [SerializeField] private Color _maxPanelCristalColor;
        [ColorUsage(false,true)] [SerializeField] private Color _minPanelCristalColor;
        
        public override void DestroyItem()
        {
            _mainCristal.gameObject.SetActive(false);
            StartCoroutine(FadePanel());
        }

        private IEnumerator FadePanel()
        {
            var overallTimeSec = 0f;
            var maxOverallTimeSec = Mathf.Max(_roofDampingDurationSec + _roofDelayTimeSec, _panelDampingDurationSec);
            
            while (overallTimeSec < maxOverallTimeSec )
            {
                yield return null;

                if (overallTimeSec < _panelDampingDurationSec )
                {
                    var percent = overallTimeSec / _panelDampingDurationSec;
                    foreach (var cristal in _panelCristals)
                    {
                        cristal.material.SetColor("_Color0", 
                            Color.Lerp(_minPanelCristalColor, _maxPanelCristalColor, 1 - _panelCristalСolorCurve.Evaluate(percent)));
                    }
                }

                if (overallTimeSec > _roofDelayTimeSec)
                {
                    var percent = (overallTimeSec - _roofDelayTimeSec) / _roofDampingDurationSec;
                    foreach (var cristal in _roofCristals)
                    {
                        cristal.material.SetColor("_EmissionColor", 
                            Color.Lerp(_minRoofCristalColor, _maxRoofCristalColor, 1 - _roofCristalСolorCurve.Evaluate(percent)));
                    }

                    foreach (var roofLight in _roofLights)
                    {
                        roofLight.intensity = _maxIntensityRoofLight * (1 - _roofCristalСolorCurve.Evaluate(percent));
                    }
                }
                overallTimeSec += Time.deltaTime;      
            }
            
            Destroy(gameObject);
        }
    }
}