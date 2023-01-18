using System;
using System.Collections;
using CodeAnimation;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class PanelUtil : MonoBehaviour
{
    [Header("Texts")] [SerializeField] private Text _eventText;
    [SerializeField] private Text _titleText;

    [SerializeField] private Animator _animator;

    [SerializeField] private AudioClip _typingClip;

    [SerializeField] private OutLineAnimation _outLineAnimation;
    [SerializeField] private DissolveAnimation _dissolveAnimation;

    private static readonly int Exit = Animator.StringToHash("Exit");

    private AudioSource _sfxSource;

    private Coroutine _shaderRoutine;
    private Coroutine _typingRoutine;

    private string _eventString;
    private string _titleString;

    public DissolveAnimation DissolveAnimation => _dissolveAnimation;
    public OutLineAnimation OutLineAnimation => _outLineAnimation;
    
    public event Action<bool> OnChangeState;

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }

    private Coroutine StartRoutine(IEnumerator routine, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        return coroutine = StartCoroutine(routine);
    }

    public void Show()
    {
        ShowText();

        _dissolveAnimation.SetImagesDissolve();

        StartRoutine(Showing(), ref _shaderRoutine);

        OnChangeState?.Invoke(true);
    }

    public void Dissolve()
    {
        _animator.SetBool(Exit, true);
        OnSkipText();

        StartRoutine(OutLiningOff(), ref _shaderRoutine);
    }

    private IEnumerator Showing()
    {
        yield return _dissolveAnimation.Emerging();

        StartRoutine(_outLineAnimation.OutLiningOn(), ref _shaderRoutine);
        StartRoutine(TypeText(), ref _typingRoutine);
    }
    
    private IEnumerator Dissolving()
    {
        HideText();

        yield return _dissolveAnimation.Dissolving();

        OnChangeState?.Invoke(false);
        gameObject.SetActive(false);
    }

    private IEnumerator OutLiningOff()
    {
        yield return _outLineAnimation.OutLiningOff();

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
}