﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [SerializeField] private float _cameraRotationTime;

        [Inject] private GameSession _session;

        private PredefinedDataGroup<EnemyCardViewWidget, EnemyPack> _dataGroup;
        private List<EnemyPack> _enemyPacks = new List<EnemyPack>();

        private Camera _mainCamera;//Выдели анимацию...
        private Coroutine _rotateRoutine;

        
        private void Awake()
        {
            textPanelUtil = _eventPanelUtil;
            
            _dataGroup = new PredefinedDataGroup<EnemyCardViewWidget, EnemyPack>(_enemyCardContainer);
        }

        protected override void Start()
        {
            base.Start();
            _mainCamera = Camera.main;
            if (_mainCamera == null) throw new NullReferenceException("Camera not found");

            _eventPanelUtil.OnChangeState += CloseEventContainer;
        }

        public override void ShowEventContainer(LevelCard card)
        {
            base.ShowEventContainer(card);

            _crowdWidget.Activate(_enemyPacks.Count > 3);
            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(-15, 0, 0), _cameraRotationTime)); //Подобную сексуальную анимацию не остановить...
        }

        public async Task PrepareToBattle(AbstractPanelUtil battlePanel)
        {
            await PutDownCamera();
            _eventPanelUtil.PrepareToBattle(battlePanel);//анимация переходящая с ивентовой панельки на панельку боевую
            battlePanel.TakePanelSubscribes(_eventPanelUtil);
        }

        public List<EnemyPack> TakeEnemyPacks()
        {
            return new List<EnemyPack>(_enemyPacks);
        }

        private Task PutDownCamera()
        {
            CloseEventContainer(false);
            return Task.Delay((int)_cameraRotationTime * 1000);
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_rotateRoutine != null)
                StopCoroutine(_rotateRoutine);

            _rotateRoutine = StartCoroutine(coroutine);
        }

        private void CloseEventContainer(bool active)
        {
            if (active)
                return; // Почему так? А потому что сначало поднять камеру, а потом анимацию провести лучше
            // смотрится, потому нам нужно известие лишь о сокрытии.

            StartRoutine(RotateAnimation.Rotate(_mainCamera.gameObject, _mainCamera.transform.rotation,
                Quaternion.Euler(0, 0, 0), _cameraRotationTime));
        }

        protected override void DynamicInitialization(LevelCard card)
        {
            base.DynamicInitialization(card);

            var battleCard = (BattleCard)card;
            if (battleCard == null) throw new ArgumentException("Was sent not BattleCard!!! Something wrong");

            _enemyPacks = EnemyCardFactory.GeneratePacks(_session.LevelTurn.Value, battleCard.EnemyPacks);

            if (_enemyPacks.Count > 6) throw new ArgumentException("There are too many enemies in Battle!!!");

            if (_enemyPacks.Count <= 3) // Здесь магическое число) - предельное количество отображаемых карт
            {
                _dataGroup.SetData(_enemyPacks);
            }
            else
            {
                var visiblePacks = _enemyPacks.Take(2).ToList(); //Заполнение видимых врагов в панели
                _dataGroup.SetData(visiblePacks);

                var hiddenPacks = _enemyPacks.Skip(2).ToList();
                _crowdWidget.SetEnemyData(hiddenPacks);
            }
        }

        private void OnDestroy()
        {
            _eventPanelUtil.OnChangeState -= CloseEventContainer;
        }
    }
}