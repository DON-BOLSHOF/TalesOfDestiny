using System;
using System.Collections;
using Cards;
using CodeAnimation;
using Definitions.Enemies;
using Panels;
using UnityEngine;
using Utils;
using View.EnemyCardView;
using Zenject;

namespace Controllers
{
    public class BattleEventManager : EventManager //Чистая инициализация(+анимация)))
    {
        [SerializeField] private BattleEventPanelUtil _eventPanelUtil;
        [SerializeField] private Transform _enemyCardContainer;

        [Inject] private GameSession _session;
        
        private PredefinedDataGroup<EnemyCardViewWidget, EnemyPack> _dataGroup;

        private Camera _mainCamera;
        private Coroutine _rotateRoutine;

        private void Awake()
        {
            _dataGroup = new PredefinedDataGroup<EnemyCardViewWidget, EnemyPack>(_enemyCardContainer);
        }

        protected override void Start()
        {
            base.Start();
            _mainCamera =
                GameObject.FindWithTag("MainCamera")
                    .GetComponent<
                        Camera>(); //Интересно zenject через сцены работает? Ага, называется projectContext дебильчик

            _eventPanelUtil.OnChangeState += CloseEventContainer;
        }

        public override void ShowEventContainer(LevelCard card)
        {
            base.ShowEventContainer(card);
            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(-15, 0, 0), 2)); //Подобную сексуальную анимацию не остановить...
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_rotateRoutine != null)
                StopCoroutine(_rotateRoutine);

            _rotateRoutine = StartCoroutine(coroutine);
        }

        private void CloseEventContainer(bool value)
        {
            if (value)
                return; // Почему так? А потому что сначало поднять камеру, а потом анимацию провести лучше
            // смотрится, потому нам нужно известие лишь о сокрытии.

            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(0, 0, 0), 2));
        }

        protected override void DynamicInitialization(LevelCard card)
        {
            base.DynamicInitialization(card);

            var battleCard = (BattleCard)card;
            if (battleCard == null) throw new ArgumentException("Was sent not BattleCard!!! Something wrong");

            var packs = EnemyCardFactory.GeneratePacks(_session.LevelTurn.Value, battleCard.EnemyPacks);
            _dataGroup.SetData(packs);
        }

        private void OnDestroy()
        {
            _eventPanelUtil.OnChangeState -= CloseEventContainer;
        }
    }
}