using CodeAnimation;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Interfaces
{
    public interface ITypingText
    {
        Text[] _texts { get; }
        AudioClip _typingClip { get; }
        TypingAnimation _typingAnimation { get; }

        public void OnSkipText();
    }
}