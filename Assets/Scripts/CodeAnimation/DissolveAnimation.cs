using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public class DissolveAnimation : SoundedAnimation
    {
        [SerializeField] private float _dissolveTime = 0.07f;
        
        [SerializeField] private Material _dissolve;

        [SerializeField] private Material _specialDissolveMaterial; //Если какие-то инстансы отдельно задизолвить надо будет, понадобится это

        [SerializeField] private List<Graphic> _changesGraphicElements; //Здесь все нужные для dissolv-a(Будет в дальнейшем заменяться поэтому нужны инстансы)

        public event Action OnEmerged;
        public event Action OnStartDissolving;

        private List<Graphic> _specialObjects;//Сейчас это пока кнопки. Задаются в скрипте. Ниже прокидываются из нижестоящих в йерархии элементов.
        private readonly List<Image>
            _dynamicMaskImages = new List<Image>(); // Оказалось придется еще динамично заполнять иконки ヽ(°□° )ノ

        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

        public override IEnumerator StartAnimation()
        {
            PlayClip();
            yield return Emerging();
        }

        public override IEnumerator EndAnimation()
        {
            PlayClip();
            yield return Dissolving();
        }
        
        private IEnumerator Emerging()
        {
            ChangeMaterial(_changesGraphicElements, _dissolve);
            SetDeactiveDissolve();

            for (float i = 0; i <= 1; i += 0.05f)
            {
                SetDissolveValue(i);
                yield return new WaitForSeconds(_dissolveTime);
            }

            OnEmerged?.Invoke();
        }

        private IEnumerator Dissolving()
        {
            ChangeMaterial(_changesGraphicElements, _dissolve);
            OnStartDissolving?.Invoke();
            SetActiveDissolve();

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                SetDissolveValue(i);
                yield return new WaitForSeconds(_dissolveTime);
            }
        }
        
        public void AddDynamicImage(Image image)
        {
            _dynamicMaskImages.Add(image);
        }

        private void SetDissolveValue(float value)
        {
            _dissolve.SetFloat(DissolveAmount, value);
            _dynamicMaskImages?.ForEach(image =>
                image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, value));
        }

        private static void ChangeMaterial(List<Graphic> images, Material material)
        {
            images?.ForEach(image => image.material = material);
        }

        public IEnumerator ReloadSpecialObjects(List<Graphic> specialObjects, Func<int> onDissolved = null,
            Func<List<Graphic>> onCheckNewObject = null) //В принципе, отбросив все доп переменные нормально, но адекватно ли так доп. переменные вводить? Макаронина...  
        {
            _specialObjects = specialObjects;

            ChangeMaterial(_specialObjects, _specialDissolveMaterial);
            
            PlayClip();
            for (float i = 1; i >= 0; i -= 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(_dissolveTime);
            }

            onDissolved?.Invoke();
            if (onCheckNewObject != null)
                _specialObjects = onCheckNewObject?.Invoke();
            ChangeMaterial(_specialObjects, _specialDissolveMaterial);
            _specialDissolveMaterial.SetFloat(DissolveAmount, 0f);

            PlayClip();
            for (float i = 0; i <= 1; i += 0.05f)
            {
                _specialDissolveMaterial.SetFloat(DissolveAmount, i);
                yield return new WaitForSeconds(_dissolveTime);
            }

            SetBaseMaterials();
        }

        public void SetActiveDissolve()
        {
            SetDissolveValue(1);
            _dynamicMaskImages?.ForEach(image =>
                image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 1f));
        }

        public void SetDeactiveDissolve()
        {
            SetDissolveValue(0);
            _dynamicMaskImages?.ForEach(image =>
                image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));
        }

        public void SetBaseMaterials()
        {
            if (_specialObjects == null) return;
            _specialDissolveMaterial.SetFloat(DissolveAmount, 1f);
            ChangeMaterial(_specialObjects, _dissolve);
        }

        private void OnDisable()
        {
            _dynamicMaskImages.Clear();
        }
    }
}