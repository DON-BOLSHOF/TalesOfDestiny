using UnityEngine;

namespace View
{
    public abstract class CardView : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _backgroundView;

        public string Id => _id;
        public Sprite BackgroundView => _backgroundView;
    }
}