using System;
using UnityEngine;
using Utils;

namespace Components.Audio
{
    public class PlaySoundComponent : MonoBehaviour
    {
        [SerializeField] private AudioData[] _sounds;
        
        private AudioSource _source;

        private void Start()
        {
            _source = AudioUtils.FindSfxSource();
        }

        public void PlayClip(string id)
        {
            foreach(var sound in _sounds)
            {
                if (sound.Id != id) continue;

                _source.PlayOneShot(sound.Clip);
                break;
            }
        }

        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;
        }
    }
}