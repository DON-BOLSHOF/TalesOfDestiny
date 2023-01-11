using System.Collections;
using Cards;
using CodeAnimation;
using Panels;
using UnityEngine;

namespace Controllers
{
    public class BattleController : EventController
    {
        [SerializeField] private EnemyPanelUtil _panelUtil;

        private Camera _mainCamera;

        protected override void Start()
        {
            base.Start();
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();//Интересно zenject через сцены работает?

            _panelUtil.OnChangeState += CloseEventContainer;
        }

        public override void Show(LevelCard card)
        {
            base.Show(card);
            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(-15, 0, 0), 2)); //Подобную сексуальную анимацию не остановить...
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            StartCoroutine(coroutine);
        }

        private void CloseEventContainer(bool value)
        {
            if (value)
                return; // Почему так? А потому что сначало поднять камеру, а потом анимацию провести лучше
                        // смотрится потому нам нужно известие лишь о сокрытии.

            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(0, 0, 0), 2));
        }

        private void OnDestroy()
        {
            _panelUtil.OnChangeState -= CloseEventContainer;
        }
    }
}