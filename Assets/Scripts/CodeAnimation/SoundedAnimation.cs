using System.Collections;
using Components.Audio;
using UnityEngine;

namespace CodeAnimation
{
    [RequireComponent(typeof(PlaySFXSound))]
    public abstract class SoundedAnimation: MonoBehaviour
    {
        [SerializeField] private PlaySFXSound _sound;
        
        private void Awake()
        {
            _sound = GetComponent<PlaySFXSound>();
        }

        public virtual IEnumerator StartAnimation()
        {
            PlayClip();
            yield return null;
        }

        public virtual IEnumerator EndAnimation()
        {
            PlayClip();
            yield return null;
        }

        protected void PlayClip()
        {
            _sound.PlayClip();
        }
    }
}