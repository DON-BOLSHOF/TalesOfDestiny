using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PanelUtil : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _dissolve;
    [SerializeField] private Material _outline;
    
    [SerializeField] private List<Image> _images;
    [SerializeField] private GameObject _particles;
    [SerializeField] private Animator _animator;
    
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    private static readonly int OutlineGlow = Shader.PropertyToID("_OutlineGlow");

    private static readonly int Exit = Animator.StringToHash("Exit");
    
    private Coroutine _routine;
    private Color _glow;

    public event Action<bool> OnChangeState;

    private void Start()
    {
        _glow =_outline.GetColor(OutlineGlow);
    }

    private void StartRoutine(IEnumerator routine)
    {
        if (_routine != null)
            _routine = null;

        _routine = StartCoroutine(routine);
    }

    public void Show()
    {
        _images.ForEach(image =>image.material = _dissolve);
        _images.ForEach(image =>image.material.SetFloat(DissolveAmount,0f));

        StartRoutine(Showing());
        
        OnChangeState?.Invoke(true);
        _particles.SetActive(true);
    }

    public void Dissolve()
    {
        _animator.SetBool(Exit,true);
        
        StartRoutine(OutLiningOff());
    }
    
    private IEnumerator Showing()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            _images.ForEach(image =>image.material.SetFloat(DissolveAmount,i));
            yield return new WaitForSeconds(0.07f);
        }
        
        _images.ForEach(image =>image.material = _outline);
        StartRoutine(OutLiningOn());
    }

    private IEnumerator OutLiningOn()
    {
        for (float i = 0; i <= 5; i += 0.25f)
        {
            _images.ForEach(image =>image.material.SetFloat(OutlineThickness,i));
            _images.ForEach(image =>image.material.SetColor(OutlineGlow, _glow * (i+9)));
            yield return new WaitForSeconds(0.07f);
        }
    }

    private IEnumerator Dissolving()
    {
        for (float i = 1; i >= 0; i -= 0.05f)
        {
            _images.ForEach(image =>image.material.SetFloat(DissolveAmount,i));
            yield return new WaitForSeconds(0.07f);
        }

        OnChangeState?.Invoke(false);
        
        gameObject.SetActive(false);
    }

    private IEnumerator OutLiningOff()
    {
        for (float i = 5; i >= 0; i -= 0.25f)
        {
            _images.ForEach(image =>image.material.SetColor(OutlineGlow, _glow *(i+9)));
            _images.ForEach(image =>image.material.SetFloat(OutlineThickness,i));
            yield return new WaitForSeconds(0.07f);
        }
        
        _outline.SetColor(OutlineGlow, _glow);
        
        _images.ForEach(image =>image.material = _dissolve);
        StartRoutine(Dissolving());
    }

    private void OnDestroy()
    {
        _outline.SetColor(OutlineGlow, _glow); // Материал как ссылка прямо изменяется в ассетах. Хз ссылки на начальные не скидываются
    }
}
