using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorShowcase : UiHint, IInventoryUseAction
    {
        [SerializeField] private GameObject _gear;
        [SerializeField] private Transform _playerTransform;
        
        [Header("Cameras")]
        [SerializeField] private GameObject _clearShotCamera;
        [SerializeField] private GameObject _showcaseCamera;

        private const int ElevatorGearId = 1;
        
        public bool OnInventoryUseAction(int itemId)
        {
            if (ElevatorGearId != itemId)
                return false;
            
            _gear.SetActive(true);
            GetComponentInParent<ElevatorController>().IsGearPlacedInMechanism = true;
            GetComponentInParent<Animator>().SetBool("IsMechanismBroken", false);
            
            OnTriggerExit(null);
            GetComponent<BoxCollider>().enabled = false;
            
            return true;
        }

        public override void OnSkipChosen()
        {
            SwitchToClearShotCamera();
        }
        protected override void OnKeyActionPressedDown()
        {
            base.OnKeyActionPressedDown();
            
            PlacePlayerNearCristal();
            SwitchToShowcaseCamera();
        }

        private void SwitchToShowcaseCamera()
        {
            _showcaseCamera.SetActive(true);
            _clearShotCamera.SetActive(false);
        }

        private void SwitchToClearShotCamera()
        {
            _clearShotCamera.SetActive(true);
            _showcaseCamera.SetActive(false);
        }

        private void PlacePlayerNearCristal()
        {
            GGameManager.Instance.Player.transform.position = _playerTransform.position;
            GGameManager.Instance.Player.transform.rotation = _playerTransform.rotation;
        }
    }
}