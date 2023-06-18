using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace GamePlay
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private Texture2D brush;
        [SerializeField] private Color32 yellowBrushColor;
        [SerializeField] private Color32 redBrushColor;
        [SerializeField] private Color32 blueBrushColor;
        private Color32 brushColor;

        private Texture2D texture;
        private Color32 baseColor = new Color32(205,205,205,205);
        private float brushSizeMultiplier = 1;

        private MeshRenderer _meshRenderer;
        public MeshRenderer MeshRenderer => _meshRenderer ? _meshRenderer : (_meshRenderer =  GetComponentInChildren<MeshRenderer>());
        
        private MeshFilter _meshFilter;
        public MeshFilter MeshFilter => _meshFilter ? _meshFilter : (_meshFilter =  GetComponentInChildren<MeshFilter>());
        
        private void Start()
        {
            texture = new Texture2D(512, 512, TextureFormat.ARGB32, false);
            MeshRenderer.sharedMaterial.mainTexture = texture;
            brushColor = yellowBrushColor;
        }

        private void Update()
        {
            if (GameManager.IsPlay)
                return;
            if (!Input.GetMouseButton(0)) return;
            
            var uvCoordinate = GetUVCoordinate();
            if (uvCoordinate == Vector2.zero)
                return;
            Paint(uvCoordinate);
        }

        public void SetBrushSizeMultiplier(float addMultiplier) => brushSizeMultiplier = (addMultiplier/3) + 1;
        public void SetYellow() => brushColor = yellowBrushColor;
        public void SetRed() => brushColor = redBrushColor;
        public void SetBlue() => brushColor = blueBrushColor;

        private Vector2 GetUVCoordinate() => Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo) 
            ? hitInfo.textureCoord : Vector2.zero;

        private void Paint(Vector2 coordinate)
        {
            Color32[] textureC32 = texture.GetPixels32();
            Color32[] brushC32 = brush.GetPixels32();

            int textureWidth = texture.width;
            int textureHeight = texture.height;
            int brushWidth = brush.width;
            int brushHeight = brush.height;

            int startX = Mathf.FloorToInt(coordinate.x * textureWidth) - Mathf.FloorToInt((brushWidth * brushSizeMultiplier) / 2f);
            int startY = Mathf.FloorToInt(coordinate.y * textureHeight) - Mathf.FloorToInt((brushHeight * brushSizeMultiplier) / 2f);

            for (int y = 0; y < brushHeight; y++)
            {
                for (int x = 0; x < brushWidth; x++)
                {
                    int textureIndex = (startY + Mathf.FloorToInt(y * brushSizeMultiplier)) * textureWidth + (startX + Mathf.FloorToInt(x * brushSizeMultiplier));
                    int brushIndex = y * brushWidth + x;

                    if (textureIndex >= 0 && textureIndex < textureC32.Length && brushIndex >= 0 && brushIndex < brushC32.Length)
                    {
                        textureC32[textureIndex] = Color32.Lerp(textureC32[textureIndex], brushColor, brushC32[brushIndex].a / 255f);
                    }
                }
            }

            texture.SetPixels32(textureC32);
            texture.Apply();

            int paintedPix = 0;
            foreach (var tex in textureC32)
            {
                if (tex.r != baseColor.r)
                    if (tex.g != baseColor.g)
                        if (tex.b != baseColor.b)
                                paintedPix++;
            }

            var percent = ((float)paintedPix / textureC32.Length) * 100;
            UIManager.Instance.UpdatePaintPercent((int)percent);
            
            if(percent > 98)
                UIManager.Instance.OnComplete();
        }
    }
}