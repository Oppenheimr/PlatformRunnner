using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay
{
    /// <summary>
    /// Performs the function of texture painting.
    /// </summary>
    public class Painter : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Texture2D brush;
        [SerializeField] private Color32 yellowBrushColor;
        [SerializeField] private Color32 redBrushColor;
        [SerializeField] private Color32 blueBrushColor;
        private Color32 brushColor;

        private Texture2D texture;
        private Color32 baseColor = new Color32(205, 205, 205, 205);
        private float brushSizeMultiplier = 1;

        #endregion

        #region Properties

        private MeshRenderer _meshRenderer;

        public MeshRenderer MeshRenderer =>
            _meshRenderer ? _meshRenderer : (_meshRenderer = GetComponentInChildren<MeshRenderer>());

        private MeshFilter _meshFilter;

        public MeshFilter MeshFilter =>
            _meshFilter ? _meshFilter : (_meshFilter = GetComponentInChildren<MeshFilter>());

        #endregion

        #region Unity Event Functions

        private void Start()
        {
            // Create a new texture with dimensions 512x512
            texture = new Texture2D(512, 512, TextureFormat.ARGB32, false);
            // Set the texture as the main texture of the mesh renderer's shared material
            MeshRenderer.sharedMaterial.mainTexture = texture;
            // Set the initial brush color to yellow
            brushColor = yellowBrushColor;
        }

        private void Update()
        {
            // Check if the game is currently in play mode
            if (GameManager.IsPlay)
                return;

            // Check if the left mouse button is being held down
            if (!Input.GetMouseButton(0))
                return;

            // Get the UV coordinate on the texture based on the mouse position
            var uvCoordinate = GetUVCoordinate();
            if (uvCoordinate == Vector2.zero)
                return;

            // Perform the painting operation
            Paint(uvCoordinate);
        }

        #endregion

        #region UI Callbacks

        public void SetBrushSizeMultiplier(float addMultiplier) => brushSizeMultiplier = addMultiplier * 1.2f + 1;
        public void SetYellow() => brushColor = yellowBrushColor;
        public void SetRed() => brushColor = redBrushColor;
        public void SetBlue() => brushColor = blueBrushColor;

        #endregion

        #region Core

        private Vector2 GetUVCoordinate() =>
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo)
                ? hitInfo.textureCoord
                : Vector2.zero;

        private void Paint(Vector2 coordinate)
        {
            // Get the pixel data of the texture and brush
            Color32[] textureC32 = texture.GetPixels32();
            Color32[] brushC32 = brush.GetPixels32();

            // Calculate the starting position for painting based on the UV coordinate and brush size
            int startX = Mathf.FloorToInt(coordinate.x * texture.width);
            int startY = Mathf.FloorToInt(coordinate.y * texture.height);

            // Calculate the scaled brush size based on brushSizeMultiplier
            int scaledBrushWidth = Mathf.FloorToInt(brush.width * brushSizeMultiplier);
            int scaledBrushHeight = Mathf.FloorToInt(brush.height * brushSizeMultiplier);

            // Calculate the offset to center the brush
            int offsetX = Mathf.FloorToInt(scaledBrushWidth / 2f);
            int offsetY = Mathf.FloorToInt(scaledBrushHeight / 2f);

            // Iterate over each pixel in the brush and apply it to the texture
            for (int y = 0; y < scaledBrushHeight; y++)
            {
                for (int x = 0; x < scaledBrushWidth; x++)
                {
                    int textureX = startX + x - offsetX;
                    int textureY = startY + y - offsetY;

                    // Check if the texture coordinates are within bounds
                    if (textureX >= 0 && textureX < texture.width && textureY >= 0 && textureY < texture.height)
                    {
                        int textureIndex = textureY * texture.width + textureX;
                        int brushX = Mathf.FloorToInt(x / brushSizeMultiplier);
                        int brushY = Mathf.FloorToInt(y / brushSizeMultiplier);
                        int brushIndex = brushY * brush.width + brushX;

                        // Apply the brush color to the texture pixel using alpha blending based on the brush alpha value
                        textureC32[textureIndex] = Color32.Lerp(textureC32[textureIndex], brushColor,
                            brushC32[brushIndex].a / 255f);
                    }
                }
            }

            // Set the modified pixel data back to the texture and apply the changes
            texture.SetPixels32(textureC32);
            texture.Apply();

            // Count the number of painted pixels
            var paintedPix = 0;
            foreach (var tex in textureC32)
            {
                if (!tex.EqualsWithTolerance(baseColor, 75))
                    paintedPix++;
            }

            // Calculate the percentage of painted pixels and update the UI
            var percent = ((float)paintedPix / textureC32.Length) * 100;
            UIManager.Instance.UpdatePaintPercent((int)percent);

            // Check if the painting is complete (98% or more painted pixels) and notify the game manager
            if (percent >= 99.5f)
                GameManager.Instance.OnComplete();
        }

        #endregion
    }
}