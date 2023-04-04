using UnityEngine;
using Utils;

namespace Components.Audio
{
    public class PlaySFXSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] [Range(0, 1f)] private float _volume; 

        private AudioSource _source;

        private void Awake()
        {
            _source = AudioUtils.FindSfxSource();
            if (_volume <= 0) _volume = _source.volume;
        }

        public void PlayClip()
        {
            _source.PlayOneShot(_clip, _volume);
        }
    }
}