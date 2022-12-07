using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Image = UnityEngine.UI.Image;

public class PanelUtil : MonoBehaviour
{
    [Header("Materials")] [SerializeField] private Material _dissolve;
    [SerializeField] private Material _outline;

    [Header("Texts")] [SerializeField] private Text _eventText;
    [SerializeField] private Text _titleText;

    [Header("View")] [SerializeField] private List<Image> _images;
    [SerializeField] private List<Image> _maskImages;
    [SerializeField] private GameObject _particles;
    [SerializeField] private Animator _animator;

    [SerializeField] private AudioClip _typingClip;

    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    private static readonly int OutlineGlow = Shader.PropertyToID("_OutlineGlow");

    private static readonly int Exit = Animator.StringToHash("Exit");

    private AudioSource _sfxSource;

    private Color _glow;

    private Coroutine _shaderRoutine;
    private Coroutine _typingRoutine;

    private string _eventString;
    private string _titleString;

    public event Action<bool> OnChangeState;

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();

        _glow = _outline.GetColor(OutlineGlow);
    }

    private void StartRoutine(IEnumerator routine, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(routine);
    }

    public void Show()
    {
        ShowText();

        _images.ForEach(image => image.material = _dissolve);
        _images.ForEach(image => image.material.SetFloat(DissolveAmount, 0f));
        _maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, 0f));

        StartRoutine(Showing(), ref _shaderRoutine);

        OnChangeState?.Invoke(true);
        _particles.SetActive(true);
    }

    public void Dissolve()
    {
        _animator.SetBool(Exit, true);

        StartRoutine(OutLiningOff(), ref _shaderRoutine);
    }

    private IEnumerator Showing()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            _images.ForEach(image => image.material.SetFloat(DissolveAmount, i));
            _maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));
            yield return new WaitForSeconds(0.07f);
        }

        StartRoutine(OutLiningOn(), ref _shaderRoutine);
        StartRoutine(TypeText(), ref _typingRoutine);
    }

    private IEnumerator OutLiningOn()
    {
        _images.ForEach(image => image.material = _outline);

        for (float i = 0; i <= 5; i += 0.25f)
        {
            _images.ForEach(image => image.material.SetFloat(OutlineThickness, i));
            _images.ForEach(image => image.material.SetColor(OutlineGlow, _glow * (i + 9)));
            yield return new WaitForSeconds(0.07f);
        }
    }

    private IEnumerator Dissolving()
    {
        _images.ForEach(image => image.material = _dissolve);

        for (float i = 1; i >= 0; i -= 0.05f)
        {
            _images.ForEach(image => image.material.SetFloat(DissolveAmount, i));
            _maskImages.ForEach(image => image.GetModifiedMaterial(image.material).SetFloat(DissolveAmount, i));

            if ((int)Math.Truncate(i * 10) == 6)
                HideText(); //Да не он должен отвечать за это действие, но здесь он на практике выглядит получше.

            yield return new WaitForSeconds(0.07f);
        }

        OnChangeState?.Invoke(false);

        gameObject.SetActive(false);
    }

    private IEnumerator OutLiningOff()
    {
        OnSkipText();

        for (float i = 5; i >= 0; i -= 0.25f)
        {
            _images.ForEach(image => image.material.SetColor(OutlineGlow, _glow * (i + 9)));
            _images.ForEach(image => image.material.SetFloat(OutlineThickness, i));
            yield return new WaitForSeconds(0.07f);
        }

        _outline.SetColor(OutlineGlow, _glow);

        StartRoutine(Dissolving(), ref _shaderRoutine);
    }

    private IEnumerator TypeText()
    {
        foreach (var symbol in _titleString)
        {
            _titleText.text += symbol;
            _sfxSource.PlayOneShot(_typingClip);
            yield return new WaitForSeconds(0.09f);
        }

        foreach (var symbol in _eventString)
        {
            _eventText.text += symbol;
            _sfxSource.PlayOneShot(_typingClip);
            yield return new WaitForSeconds(0.09f);
        }
    }

    public void OnSkipText()
    {
        if (_typingRoutine != null)
        {
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;

            _titleText.text = _titleString;
            _eventText.text = _eventString;
        }
    }

    private void ShowText()
    {
        _eventString = _eventText.text;
        _eventText.text = string.Empty;
        _titleString = _titleText.text;
        _titleText.text = string.Empty;
    }

    private void HideText()
    {
        _eventText.text = string.Empty;
        _titleText.text = string.Empty;
    }

    private void OnDestroy()
    {
        _outline.SetColor(OutlineGlow,
            _glow); // Материал как ссылка прямо изменяется в ассетах. Хз ссылки на начальные не скидываются
    }
}