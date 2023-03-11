using Definitions.Creatures.Enemies;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace View.EnemyCardView
{
    public class EnemyCardViewWidget : ItemWidgetView, IItemInstance<EnemyPack>
    {
        [Space][Header("OffSet")]
        [SerializeField] private float widgetOffsetCorrection;

        [Space] [Header("Texts")]
        [SerializeField] private Text _turnThesholdValue;
        [SerializeField] private Text _enemyAmountValue;
        [SerializeField] private Text _attackValue;
        [SerializeField] private Text _healthValue;
        
        [Space][Header("Icons")]
        [SerializeField] protected Image _itemIcon;
        
        [Space] [Header("Banners")]
        [SerializeField] protected Text _nameBanner;
        [SerializeField] protected Text _descriptionBanner;
        
        [Space][Header("Types")]
        [SerializeField] private Image _attackType;
        [SerializeField] private Image _armorType;

        [Space][Header("Sprites")]
        [SerializeField] private Sprite[] _attackSprites;
        [SerializeField] private Sprite[] _armorSprites;
        

        public void SetData(EnemyPack pack)
        {
            var packEnemyCard = pack.CreatureCard;
            SetViewData(packEnemyCard.View);

            _turnThesholdValue.text = ((EnemyCard)packEnemyCard).TurnThreshold.ToString();
            _enemyAmountValue.text = pack.Count.ToString();
            _attackValue.text = packEnemyCard.Attack.ToString();
            _healthValue.text = packEnemyCard.Health.ToString();

            _attackType.sprite = _attackSprites[(int)packEnemyCard.AttackType];
            _armorType.sprite = _armorSprites[(int)packEnemyCard.ArmorType];
        }

        public override void SetViewData(CardView view)
        {
            base.SetViewData(view);
            var enemyCardView = (EnemyCardView)view;

            _itemIcon.sprite = enemyCardView.MainView;
            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + enemyCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять

            _nameBanner.text = enemyCardView.Id;
            _descriptionBanner.text = enemyCardView.Wisecrack;
        }
    }
}