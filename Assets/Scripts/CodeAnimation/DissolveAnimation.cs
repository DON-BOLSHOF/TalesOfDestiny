using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public class DissolveAnimation : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        [SerializeField] private Material _dissolve;

        public event Action IEmerged;
        public event Action IStartDissolving;
        
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        
        private readonly List<Image> _dynamicMaskImages = new List<Image>(); // Оказалось придется еще динамично заполнять иконки ヽ(°□° )ノ

        public void AddDynamicImage(Image image)
        {
            _dynamicMaskImages.Add(image);
        }
        
        public void SetImagesDissolve()
        {
            _images.ForEach(image => image.material = _dissolve);
            _images.ForEach(image => image.material.SetFloat(DissolveAmount, 0f));
            //_maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));
            _dynamicMaskImages?.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));
        }
        
        public IEnumerator Emerging()
        {
            for (float i = 0; i <= 1; i += 0.05f)
            {
                _images.ForEach(image => image.material.SetFloat(DissolveAmount, i));
                //_maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));
                _dynamicMaskImages?.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));
                yield return new WaitForSeconds(0.07f);
            }
            
            IEmerged?.Invoke();
        }

        public IEnumerator Dissolving()
        {
            IStartDissolving?.Invoke();
            _images.ForEach(image => image.material = _dissolve);

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                _images.ForEach(image => image.material.SetFloat(DissolveAmount, i));
                //_maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));
                _dynamicMaskImages?.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));

                yield return new WaitForSeconds(0.07f);
            }
        }
        
        private void OnDisable()
        {
            _dynamicMaskImages.Clear();
        }
    }
}