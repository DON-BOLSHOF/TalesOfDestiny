using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public class DissolveAnimation : MonoBehaviour
    {
       [SerializeField] private List<Image> _images; //Здесь все нужные для dissolv-a
        [SerializeField] private Material _dissolve;

        [SerializeField] private Material _specialDissolveMaterial; //Там ебатория с ссылками

        public event Action OnEmerged;
        public event Action OnStartDissolving;

        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

        private readonly List<Image>
            _dynamicMaskImages = new List<Image>(); // Оказалось придется еще динамично заполнять иконки ヽ(°□° )ノ

        public void AddDynamicImage(Image image)
        {
            _dynamicMaskImages.Add(image);
        }

        public void SetImagesDissolve()
        {
            _images.ForEach(image => image.material = _dissolve);
            _dissolve.SetFloat(DissolveAmount, 0f);
            _dynamicMaskImages?.ForEach(image =>
                image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));
        }

        public IEnumerator Emerging()
        {
            for (float i = 0; i <= 1; i += 0.05f)
            {
                _dissolve.SetFloat(DissolveAmount, i);
                _dynamicMaskImages?.ForEach(image =>
                    image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));
                yield return new WaitForSeconds(0.07f);
            }

            OnEmerged?.Invoke();
        }

        public IEnumerator Dissolving()
        {
            OnStartDissolving?.Invoke();
            _images.ForEach(image => image.material = _dissolve);

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                _dissolve.SetFloat(DissolveAmount, i);
                _dynamicMaskImages?.ForEach(image =>
                    image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));

                yield return new WaitForSeconds(0.07f);
            }
        }

        public IEnumerator ReloadSpecialObjects(List<Graphic> specialObjects, Func<int> onDissolved = null,
            Func<List<Graphic>> onCheckNewObject =
                null) //В принципе, отбросив все доп переменные нормально, но адекватно ли так доп. переменные вводить? Макаронина...  
        {
            specialObjects.ForEach(image => image.material = _specialDissolveMaterial);

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(0.07f);
            }

            onDissolved?.Invoke();
            if (onCheckNewObject != null)
                specialObjects = onCheckNewObject?.Invoke();
            specialObjects.ForEach(image => image.material = _specialDissolveMaterial);
            _specialDissolveMaterial.SetFloat(DissolveAmount, 0f);

            for (float i = 0; i <= 1; i += 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(0.07f);
            }

            specialObjects.ForEach(image => image.material = _dissolve);
        }

        private void OnDisable()
        {
            _dynamicMaskImages.Clear();
        }
    }
}