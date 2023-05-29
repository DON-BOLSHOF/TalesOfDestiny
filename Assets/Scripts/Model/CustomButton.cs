using System;
using Cards.SituationCards.Event;
using Model.Tributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class CustomButton
    {
        [SerializeField] private string _name;
        [SerializeField] private CustomButtonType _customButtonType;

        [ShowIf(nameof(_customButtonType), CustomButtonType.TributeButton), SerializeField]
        private Tribute tribute;
        [SerializeField] private ButtonInteraction _buttonInteraction;

        public string Name => _name;
        public CustomButtonType ButtonType => _customButtonType;
        public Tribute Tribute => tribute;
        public ButtonInteraction ButtonInteraction => _buttonInteraction;

        public enum CustomButtonType
        {
            CommonButton,
            TributeButton
        }
    }
}