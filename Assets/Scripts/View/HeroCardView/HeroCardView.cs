using UnityEngine;

namespace View.HeroCardView
{
    [CreateAssetMenu(menuName = "CardView/HeroCardView", fileName = "HeroCardView")]
    public class HeroCardView : CardView
    {
        [SerializeField] private string _wiseCrack;

        public string WiseCrack => _wiseCrack;
    }
}