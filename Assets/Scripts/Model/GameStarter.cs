using System;
using System.Collections;
using CodeAnimation;
using Components;
using UnityEngine;
using UnityEngine.UI;

namespace Model
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private SceneLoaderComponent _sceneLoaderComponent;
        [SerializeField] private GameStarterAnimation _starterAnimation;

        public IEnumerator RunGame()
        {
            StartCoroutine(_starterAnimation.DissolveBackGround());
            yield return _starterAnimation.PrepareBattle();
            
            _sceneLoaderComponent.SceneLoad();
        }

        [Serializable]
        private class GameStarterAnimation
        {
            [SerializeField] private int _animationTime = 4;
            [SerializeField] private GameObject[] _objectsToDeactivate;
            [SerializeField] private Image _backGround;

            private Camera _mainCamera;
            
            public IEnumerator PrepareBattle()
            {
                foreach (var gameObject in _objectsToDeactivate)
                {
                    gameObject.SetActive(false);
                }

                if (_mainCamera == null) _mainCamera = Camera.main;
                
                yield return RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                    Quaternion.Euler(0, 0, 0), _animationTime); //Подобную сексуальную анимацию не остановить...
            }

            public IEnumerator DissolveBackGround()
            {
                var currentAlpha = _backGround.color.a;

                for (var i = currentAlpha; i >= 0; i -= 0.02f)
                {
                    _backGround.color = new Color(_backGround.color.r,_backGround.color.g,_backGround.color.b,i);
                    yield return new WaitForSeconds(0.07f);
                }
            }
        }
    }
}