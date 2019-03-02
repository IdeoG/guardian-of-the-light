using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class LightTransformation : MonoBehaviour, ILightTransformation
    {    
        [Header("Color")] 
        [ColorUsage(false,true)] [SerializeField] private Color minHealthMeshColor;
        [SerializeField] private List<LightEmitter> lightEmitters;

        [Header("Light")] 
        [SerializeField] private Light light;
        [SerializeField] private  AnimationCurve lightIntensityCurve;
        [SerializeField] private float maxLightIntensity;
        
        [Header("Collisions")]
        [SerializeField] private int particleCollisions;
        [SerializeField] private int maxParticleCollisions;

        private uint _collectedParticles;


        public void ReduceHealth() { }

        public bool IsHealthFull() => particleCollisions == maxParticleCollisions;
        public bool IsHealthEmpty() => particleCollisions == 0;

        private void Awake()
        {
            if (!gameObject.GetComponent<Collider>().isTrigger)
                throw new ArgumentException($"Collider with name {name} didn't set as trigger");

            UpdateLightning();
        }

        private void OnParticleCollision(GameObject other)
        {
            particleCollisions += maxParticleCollisions > particleCollisions ? 1 : 0;
            UpdateLightning();
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
                    .SetColor("_EmissionColor", Color.Lerp(minHealthMeshColor, lightEmitter.maxHealthMeshColor, curvedPercent));
            }
        }
    }

    [Serializable]
    class LightEmitter
    {
        public Renderer meshRender;
        public AnimationCurve colorCurve;
        [ColorUsage(false,true)] public Color maxHealthMeshColor;
    }
}