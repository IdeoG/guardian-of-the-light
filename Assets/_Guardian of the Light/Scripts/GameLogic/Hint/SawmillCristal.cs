using System.Collections.Generic;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class SawmillCristal : UiHint
    {
        [SerializeField] private MeshRenderer _mainCristall;
        [SerializeField] private List<MeshRenderer> _panelCristals;
        
        public override void DestroyItem()
        {
            
        }
    }
}