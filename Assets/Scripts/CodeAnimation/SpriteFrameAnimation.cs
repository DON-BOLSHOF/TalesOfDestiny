using System.Collections;
using UnityEngine;

namespace CodeAnimation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFrameAnimation : MonoBehaviour
    {
        [SerializeField] private float _frameTime = 0.07f;
        [SerializeField] private Sprite[] _sprites;

        private SpriteRenderer _mainSprite;

        private void Start()
        {
            _mainSprite = GetComponent<SpriteRenderer>();

            if (_sprites.Length == 0)
            {
                Debug.LogError("I havent sprites!!!");
                return;
            }

            StartCoroutine(SpriteAnimation());
        }

        private IEnumerator SpriteAnimation()
        {
            while (true)
            {
                foreach (var sprite in _sprites)
                {
                    _mainSprite.sprite = sprite;
                    yield return new WaitForSeconds(_frameTime);
                }
            }
        }
    }
}