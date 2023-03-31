using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    [Serializable]
    public class OutLineAnimation : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        [SerializeField] private Material _outline;
        
        private Color _glow;
        
        private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
        private static readonly int OutlineGlow = Shader.PropertyToID("_OutlineGlow");

        private void Start()
        {
            _glow = _outline.GetColor(OutlineGlow);
        }

        public IEnumerator OutLiningOn()
        {
            _images.ForEach(image => image.material = _outline);

            for (float i = 0; i <= 5; i += 0.25f)
            {
                _images.ForEach(image => image.material.SetFloat(OutlineThickness, i));
                _images.ForEach(image => image.material.SetColor(OutlineGlow, _glow * (i + 9)));
                yield return new WaitForSeconds(0.07f);
            }
        }
        
        public IEnumerator OutLiningOff()
        {
            _images.ForEach(image => image.material = _outline);

            for (float i = 5; i >= 0; i -= 0.25f)
            {
                _images.ForEach(image => image.material.SetColor(OutlineGlow, _glow * (i + 9)));
                _images.ForEach(image => image.material.SetFloat(OutlineThickness, i));
                yield return new WaitForSeconds(0.07f);
            }

            _outline.SetColor(OutlineGlow, _glow);
        }
        
        private void OnDestroy()
        {
            _outline.SetColor(OutlineGlow,
                _glow); // Материал как ссылка прямо изменяется в ассетах. Хз ссылки на начальные не скидываются
        }
    }
}