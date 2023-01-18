using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public class TypingAnimation
    {
        private AudioSource _sfxSource;
        private AudioClip _typingClip;

        private Text[] _texts;
        private string[] _stringToTyping;

        public TypingAnimation(AudioSource sfxSource, AudioClip clip, Text[] texts)
        {
            _sfxSource = sfxSource;
            _typingClip = clip;
            _texts = texts;

            _stringToTyping = new string[texts.Length];
            TakeText();
        }

        public IEnumerator TypeText()
        {
            for (int i = 0; i < _texts.Length; i++)
            for (int j = 0; j < _stringToTyping[i].Length; j++)
            {
                _texts[i].text += _stringToTyping[i][j];

                _sfxSource.PlayOneShot(_typingClip);
                yield return new WaitForSeconds(0.09f);
            }
        }

        private void TakeText()
        {
            for (int i = 0; i < _texts.Length; i++)
                _stringToTyping[i] = _texts[i].text;
        }

        public void HideText()
        {
            foreach (var text in _texts)
            {
                text.text = string.Empty;
            }
        }

        public void SkipText()
        {
            for (int i = 0; i < _texts.Length; i++)
                _texts[i].text = _stringToTyping[i];
        }
    }
}