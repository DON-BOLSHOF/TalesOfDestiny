using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public class DissolveAnimation : MonoBehaviour
    {
        [SerializeField] private List<Graphic> _images; //Здесь все нужные для dissolv-a(Будет в дальнейшем заменяться поэтому нужны инстансы)
        [SerializeField] private Material _dissolve;

        [SerializeField] private Material _specialDissolveMaterial; //Там ебатория с ссылками

        public event Action OnEmerged;
        public event Action OnStartDissolving;

        private List<Graphic> _specialObjects;

        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

        private readonly List<Image>
            _dynamicMaskImages = new List<Image>(); // Оказалось придется еще динамично заполнять иконки ヽ(°□° )ノ

        public void AddDynamicImage(Image image)
        {
            _dynamicMaskImages.Add(image);
        }

        public void SetImagesDissolve()
        {
            ChangeMaterial(_images, _dissolve);
            _dissolve.SetFloat(DissolveAmount, 0f);
            _dynamicMaskImages?.ForEach(image =>
                image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));
        }

        private void ChangeMaterial(List<Graphic> images, Material material)
        {
            images.ForEach(image => image.material = material);
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
            ChangeMaterial(_images, _dissolve);

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
            _specialObjects = specialObjects;
            
            ChangeMaterial(_specialObjects, _specialDissolveMaterial);

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(0.07f);
            }

            onDissolved?.Invoke();
            if (onCheckNewObject != null)
                _specialObjects = onCheckNewObject?.Invoke();
            ChangeMaterial(_specialObjects, _specialDissolveMaterial);
            _specialDissolveMaterial.SetFloat(DissolveAmount, 0f);

            for (float i = 0; i <= 1; i += 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(0.07f);
            }

            SetBaseMaterials();
        }

        public void SetBaseMaterials()
        {
            if(_specialObjects != null)
                ChangeMaterial(_specialObjects, _dissolve);
        }

        private void OnDisable()
        {
            _dynamicMaskImages.Clear();
        }
    }
}