using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts
{
    public class CandleLightTransformation : MonoBehaviour, ILightTransformation
    {
        [Header("Color")] [ColorUsage(false, true)] [SerializeField]
        private Color minHealthMeshColor;

        [SerializeField] private List<LightEmitter> lightEmitters;

        [Header("Light")] [SerializeField] private Light light;
        [SerializeField] private AnimationCurve lightIntensityCurve;
        [SerializeField] private float maxLightIntensity;

        [Header("Collisions")] [SerializeField]
        private int particleCollisions;

        [SerializeField] private int maxParticleCollisions;

        private uint _collectedParticles;


        public void ReduceHealth() {}

        public bool IsHealthFull() => particleCollisions == maxParticleCollisions;
        public bool IsHealthEmpty() => particleCollisions == 0;

        private void Awake()
        {
            if (!gameObject.GetComponent<Collider>().isTrigger)
                throw new ArgumentException($"Collider with name {name} didn't set as trigger");

            UpdateLightning();
            burstRealTime = burstDuration;
        }

        private void OnParticleCollision(GameObject other)
        {
            if (particleCollisions == maxParticleCollisions) return;

            particleCollisions += 1;
            IgniteCandle();
//            UpdateLightning();
        }

        public void UpdateLightning()
        {
            var percent = particleCollisions / (float) maxParticleCollisions;
            var lightCurvedPercent = lightIntensityCurve.Evaluate(percent);
            light.intensity = maxLightIntensity * lightCurvedPercent;

            foreach (var lightEmitter in lightEmitters)
            {
                var curvedPercent = lightEmitter.colorCurve.Evaluate(percent);
                lightEmitter.meshRender.material
                    .SetColor("_EmissionColor",
                        Color.Lerp(minHealthMeshColor, lightEmitter.maxHealthMeshColor, curvedPercent));
            }
        }

        [SerializeField] private AnimationCurve intensityOverTime;
        [SerializeField] private AnimationCurve emissionOverTime;
        [SerializeField] private AnimationCurve startSizeOverTime;

        [SerializeField] private Light candleLight;
        [SerializeField] private ParticleSystem candleParticles;

        [SerializeField] private float maxIntensity;
        [SerializeField] private float maxEmissionRate;
        [SerializeField] private float maxStartSize;

        [SerializeField] private float burstDuration;
        [SerializeField] private float burstRealTime;
        
        private void IgniteCandle()
        {
//            candleLight.DOIntensity(maxIntensity, burstDuration).SetEase(intensityOverTime);
            burstRealTime = 0;
        }

        private void Update()
        {
            if (burstRealTime < burstDuration)
            {
                burstRealTime += Time.deltaTime;
                candleParticles.emissionRate = maxEmissionRate * emissionOverTime.Evaluate(burstRealTime / burstDuration);
                candleParticles.startSize = maxStartSize * startSizeOverTime.Evaluate(burstRealTime / burstDuration);
                candleLight.intensity = maxIntensity * intensityOverTime.Evaluate(burstRealTime / burstDuration);
            }
        }
    }
}