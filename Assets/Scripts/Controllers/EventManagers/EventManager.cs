using System;
using Cards;
using Cards.SituationCards;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Disposables;
using Utils.Interfaces.Visitors;
using Widgets.BoardWidgets;
using Widgets.EventManagersWidgets;
using Zenject;

namespace Controllers.EventManagers
{
    public class EventManager : LevelManager, IGameStateVisitor //Чистая инициализация, так что свое ко-ко-ко о менеджере не надо мне тут, Уже обманщик получается
    {
        [SerializeField] private Text _contents;
        [SerializeField] private Text _eventText;
        [SerializeField] private CustomButtonWidget _customButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private PanelUtilItemWidget _panelUtilItemWidget; //Что это? Пойми, название смени!

        [Inject] private DiContainer _diContainer;
        [Inject] protected GameSession _session;
        [Inject] private ItemWidgetFactory _itemWidgetFactory;

        private ForwardDiDataGroup<CustomButtonWidget, CustomButton> _dataGroup;

        protected readonly DisposeHolder _trash = new DisposeHolder();

        protected virtual void Start()
        {
            _dataGroup =
                new ForwardDiDataGroup<CustomButtonWidget, CustomButton>(_customButtonPrefab, _buttonContainer,
                    _diContainer);

            _textPanelUtil.OnReloadButtons += UpdateButtons;
            _trash.Retain(new ActionDisposable(()=> _textPanelUtil.OnReloadButtons += UpdateButtons));
            _trash.Retain(_textPanelUtil.SubscribeOnChange(delegate(bool b) {VisitGameState(_session.GameStateAnalyzer, b ? Stage.Start : Stage.End); }));
        }

        public override void ShowEventContainer(LevelCard card)
        {
            DynamicInitialization(card);

            base.ShowEventContainer(card);
        }

        protected virtual void DynamicInitialization(LevelCard card)
        {
            var eventCard = (EventCard)card;
            if (eventCard == null) throw new ArgumentException("Was sent not EventCard!!!");

            _itemWidgetFactory.FulFillItemWidget(_panelUtilItemWidget, WidgetType.PanelUtil, card);

            _contents.text = eventCard.Situation.SituationName;
            _eventText.text = eventCard.Situation.Description;
            _dataGroup.SetData(eventCard.Situation.Buttons);
            _panelUtilItemWidget.View.SetViewData(eventCard.View);
        }

        public void UpdateButtons(CustomButton[] buttons)
        {
            _dataGroup.SetData(buttons);
        }

        public void VisitGameState(GameStateAnalyzer gameStateAnalyzer, Stage stage)
        {
            gameStateAnalyzer.Visit(this, stage);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}