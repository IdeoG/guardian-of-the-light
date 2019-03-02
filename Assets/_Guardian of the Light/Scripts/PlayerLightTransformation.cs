using System.Collections.Generic;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class PlayerLightTransformation : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private float forceMultiplier;

        [SerializeField] private bool isButtonEnhanceDown;
        [SerializeField] private bool isButtonReduceDown;

        private List<LightTransformationData> _lightTransformations;

        private void Update()
        {
            isButtonEnhanceDown = InputSystem.Instance._actionKey.GetKey();
            isButtonReduceDown = InputSystem.Instance._extraActionKey.GetKey();

            foreach (var lightTransformationData in _lightTransformations)
            {
                var direction = lightTransformationData.Target.position - transform.position;
                var localDirection = Quaternion.Inverse(transform.parent.localRotation) * direction;

                lightTransformationData.ForceField.directionX = localDirection.x * forceMultiplier;
                lightTransformationData.ForceField.directionY = localDirection.y * forceMultiplier;
                lightTransformationData.ForceField.directionZ = localDirection.z * forceMultiplier;
               
                lightTransformationData.ParticleSystem.emissionRate = 
                    isButtonEnhanceDown && !lightTransformationData.LightTransformation.IsHealthFull() ? 20f : 0f;
                
//                if (isButtonReduceDown) lightTransformationData.LightTransformation.
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var lightTransformation = other.gameObject.GetComponent<ILightTransformation>();

            if (lightTransformation != null)
            {
                var particlesClone = Instantiate(particles.gameObject, transform);
                var data = new LightTransformationData
                {
                    Target = other.gameObject.transform,
                    Particles = particlesClone,
                    ParticleSystem = particlesClone.GetComponent<ParticleSystem>(),
                    ForceField = particlesClone.GetComponent<ParticleSystemForceField>(),
                    LightTransformation = lightTransformation
                };

                _lightTransformations.Add(data);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var lightTransformation = other.gameObject.GetComponent<ILightTransformation>();

            if (lightTransformation != null)
            {
                foreach (var lightTransformationData in _lightTransformations)
                {
                    if (lightTransformationData.Target.Equals(other.transform))
                    {
                        _lightTransformations.Remove(lightTransformationData);
                        Destroy(lightTransformationData.Particles);
                    }
                }
            }
//            lightTransformation?.ReleaseWatchByHero();
        }

        private void Awake()
        {
            _lightTransformations = new List<LightTransformationData>();
//            particles.gameObject.SetActive(false);
            particles.emissionRate = 0f;
        }
    }
    
    /**
     * 1. Вошли в триггер - добавили источник света в Лист - создали для него копию частиц и выключили её
     * 2. Нажали кнопку - включили все частицы  частицы и там же скорректировали направление частиц известной компоненты
     * в сторону трансформа
     */

    struct LightTransformationData
    {
        public Transform Target;
        public GameObject Particles;
        public ParticleSystem ParticleSystem;
        public ParticleSystemForceField ForceField;
        public ILightTransformation LightTransformation;
    } 
}
