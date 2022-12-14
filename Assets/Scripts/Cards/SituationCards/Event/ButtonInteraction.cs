using System;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.PropertyEvents;
using LevelManipulation;
using Model.Data;
using UnityEngine;

namespace Cards.SituationCards.Event
{
    [Serializable]
    public class ButtonInteraction
    {
        [SerializeField] private EventType _type;
        [SerializeField] private PropertyEvent[] _propertyEvent;
        [SerializeField] private ArmyEvent[] _armyEvent;

        public PlayerDataButton SetPlayerData(PlayerData data)
        {
            return new PlayerDataButton(data, this);
        }

        public PanelUtilButton SetPanelButton(PanelUtil panelUtil)
        {
            return new PanelUtilButton(panelUtil, this);
        }

        public LevelManagerButton SetLevelManagerButton(LevelManager manager)
        {
            return new LevelManagerButton(manager, this);
        }

        public class PlayerDataButton
        {
            private PlayerData _data;
            private ButtonInteraction _interaction;

            public PlayerDataButton(PlayerData data, ButtonInteraction interaction)
            {
                _data = data;
                _interaction = interaction;
            }

            public void OnClick() // Сделал обязательную обертку для визитеров playerDat-ы
            {
                if ((_interaction._type & EventType.PropertyVisitor) == EventType.PropertyVisitor)
                    foreach (var propertyEvent in _interaction._propertyEvent)
                    {
                        propertyEvent.Accept(_data.HeroData);
                    }
            }
        }
        
        public class PanelUtilButton // В дальнейшем выдели базовый класс и все переопредели при рефакторинге!
        {
            private PanelUtil _util;
            private ButtonInteraction _interaction;
            
            public PanelUtilButton(PanelUtil panelUtil, ButtonInteraction interaction)
            {
                _util = panelUtil;
                _interaction = interaction;
            }

            public void OnClick()//Ну кнопка вообще не должна знать о playerData игрока, но ...
            {
                if ((_interaction._type & EventType.ClosePanel) == EventType.ClosePanel)
                    _util.Dissolve();
            }
        }
        
        public class LevelManagerButton
        {
            private LevelManager _manager;
            private ButtonInteraction _interaction;

            public LevelManagerButton(LevelManager manager, ButtonInteraction interaction)
            {
                _manager = manager;
                _interaction = interaction;
            }

            public void OnClick()
            {
                if ((_interaction._type & EventType.EndJourney) == EventType.EndJourney)
                    _manager.Reload();
            }
        }
    }

    [Flags]
    public enum EventType : short
    {
        None = 0,
        ArmyVisitor = 1,
        PropertyVisitor = 2,
        EquipVisitor = 4,
        ContinueEvent = 8,
        ClosePanel = 16,
        EndJourney = 32, 
        Everything = 63
    }
}