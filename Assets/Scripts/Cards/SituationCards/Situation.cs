using Model;
using UnityEngine;

namespace Cards.SituationCards
{
    [CreateAssetMenu(fileName = "Situation", menuName = "Defs/Other/Situation")]
    public class Situation : ScriptableObject
    {
        [SerializeField] private string _situationName;
        [SerializeField] private string _situationDescription;
        [SerializeField] private CustomButton[] _buttons;

        public string SituationName => _situationName;
        public string Description => _situationDescription;
        public CustomButton[] Buttons => _buttons;
    }
}