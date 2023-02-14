using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using CodeAnimation;
using Definitions.Enemies;
using Panels;
using UnityEngine;
using Utils;
using View.EnemyCardView;
using Widgets;
using Zenject;

namespace Controllers
{
    public class BattleEventManager : EventManager //Чистая инициализация(+анимация)))
    {
        [SerializeField] private BattleEventPanelUtil _eventPanelUtil;
        [SerializeField] private Transform _enemyCardContainer;

        [SerializeField] private EnemyCrowdWidget _crowdWidget;
        
        [Inject] private GameSession _session;

        private PredefinedDataGroup<EnemyCardViewWidget, EnemyPack> _dataGroup;
        private List<EnemyPack> _packs;

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
            
            if (_mainCamera == null) throw new NullReferenceException("Camera not found");

            _eventPanelUtil.OnChangeState += CloseEventContainer;
        }

        public override void ShowEventContainer(LevelCard card)
        {
            base.ShowEventContainer(card);
            
            _crowdWidget.Activate(_packs.Count>3);
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

            _packs = EnemyCardFactory.GeneratePacks(_session.LevelTurn.Value, battleCard.EnemyPacks);

            if (_packs.Count > 6) throw new ArgumentException("There are too many enemies in Battle!!!");
            
            if (_packs.Count <= 3) // Здесь магическое число) - предельное количество отображаемых карт
            {
                _dataGroup.SetData(_packs);
            }
            else
            {
                var visiblePacks = _packs.Take(2).ToList();//Заполнение видимых врагов в панели
                _dataGroup.SetData(visiblePacks);

                var hiddenPacks = _packs.Skip(2).ToList();
                _crowdWidget.SetEnemyData(hiddenPacks);
            }
        }
        private void OnDestroy()
        {
            _eventPanelUtil.OnChangeState -= CloseEventContainer;
        }
    }
}