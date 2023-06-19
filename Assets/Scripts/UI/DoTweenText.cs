using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Adds features to texts with DoTween plugin.
    /// </summary>
    public class DoTweenText : MonoBehaviour
    {
        [SerializeField] private float scaleMultiplier = 1.2f;
        [SerializeField] private float duration = 0.3f;
        private bool _onAnimate;

        private TextMeshProUGUI _textMesh;

        public TextMeshProUGUI TextMesh =>
            _textMesh ? _textMesh : (_textMesh = GetComponentInChildren<TextMeshProUGUI>());

        private void Awake() => DOTween.Init(); // Initializes the DOTween library.

        public void SetText(string text)
        {
            // If an animation is already in progress, abort the process.
            if (_onAnimate)
                return; 
            _onAnimate = true; // Set the flag to indicate that an animation is in progress.
            
            // Starts the scaling animation and calls the ReScale method when it's completed.
            transform.DOScale(transform.localScale * scaleMultiplier, duration).OnComplete(ReScale); 
            TextMesh.text = text; // Updates the text of the TextMeshProUGUI component with the specified text.
        }

        /// <summary>
        /// Starts the reverse scaling animation and sets the _onAnimate flag to false when it's completed.
        /// </summary>
        private void ReScale() => transform.DOScale(transform.localScale / scaleMultiplier, duration)
            .OnComplete(() => _onAnimate = false); 

    }
}