using System;
using Cards.SituationCards.Event;
using UnityEngine;

namespace Cards.SituationCards
{
    [CreateAssetMenu(fileName = "Situation", menuName = "Defs/Other/Situation")]
    public class Situation : ScriptableObject
    {
        [SerializeField] private string _situationDescription;
        [SerializeField] private CustomButton[] _buttons;

        public string Description => _situationDescription;
        public CustomButton[] Buttons => _buttons;
    }

    [Serializable]
    public class CustomButton
    {
        [SerializeField] private string _name;
        [SerializeField] private ButtonInteraction _buttonInteraction;

        public string Name => _name;
        public ButtonInteraction ButtonInteraction => _buttonInteraction;
    }
}