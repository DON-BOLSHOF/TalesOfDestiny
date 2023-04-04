using UnityEngine;

namespace View.CreaturesCardView
{
    public abstract class CreatureCardView : PortraitCardView
    {
        [SerializeField] private string _wisecrack;
        public string Wisecrack => _wisecrack;
    }
}