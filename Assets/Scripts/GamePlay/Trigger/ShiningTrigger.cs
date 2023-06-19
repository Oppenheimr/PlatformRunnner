using System.Collections;
using UnityEngine;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to add collider feature for shining obstacles.
    /// </summary>
    public class ShiningTrigger : ObstacleTrigger
    {
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material shiningMaterial;
        
        private MeshRenderer _renderer;
        public MeshRenderer Renderer => _renderer ? _renderer : (_renderer = GetComponentInChildren<MeshRenderer>());
        
        protected override void OnEnter(Transform other)
        {
            base.OnEnter(other);
            StartCoroutine(ChangeMaterial());
        }

        private IEnumerator ChangeMaterial()
        {
            Renderer.sharedMaterial = shiningMaterial;
            yield return new WaitForSeconds(1.2f);
            Renderer.sharedMaterial = normalMaterial;
        }
    }
}