using Definitions.Creatures.Enemies;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace View.CreaturesCardView.EnemyCardView
{
    public class EnemyCardViewWidget : CreatureCardViewWidget<EnemyPack>
    {
        [SerializeField] private Text _turnThesholdValue;

        public override void SetData(EnemyPack pack)
        {
            var packEnemyCard = pack.CreatureCard;
            _turnThesholdValue.text = ((EnemyCard)packEnemyCard).TurnThreshold.ToString();
            
            base.SetData(pack);
        }
    }
}