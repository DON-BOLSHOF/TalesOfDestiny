using UnityEngine;

namespace View
{
    public abstract class CardView : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _cardView;
        [SerializeField] private Sprite _backgroundView;

        public string Id => _id;
        public Sprite MainView => _cardView;
        public Sprite BackgroundView => _backgroundView;
    }
}