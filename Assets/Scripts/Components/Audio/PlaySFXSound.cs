using UnityEngine;
using Utils;

namespace Components.Audio
{
    public class PlaySFXSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        private AudioSource _source;

        private void Awake()
        {
            _source = AudioUtils.FindSfxSource();
        }

        public void PlayClip()
        {
            _source.PlayOneShot(_clip);
        }
    }
}