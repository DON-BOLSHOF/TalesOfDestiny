using Definitions.Creatures;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace View.CreaturesCardView
{
    public abstract class CreatureCardViewWidget<TCreaturePack> : CardViewWidget, IItemInstance<TCreaturePack>  where TCreaturePack: CreaturePack
    {
        [Space][Header("OffSet")]
        [SerializeField] private float widgetOffsetCorrection;

        [Space] [Header("Texts")]
        [SerializeField] private Text _amountValue;
        [SerializeField] private Text _attackValue;
        [SerializeField] private Text _healthValue;
        
        [Space][Header("Icons")]
        [SerializeField] private Image _background;
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
        

        public virtual void SetData(TCreaturePack pack)
        {
            var packEnemyCard = pack.CreatureCard;
            SetViewData(packEnemyCard.View);

            _amountValue.text = pack.Count.ToString();
            _attackValue.text = packEnemyCard.Attack.ToString();
            _healthValue.text = packEnemyCard.Health.ToString();

            _attackType.sprite = _attackSprites[(int)packEnemyCard.AttackType];
            _armorType.sprite = _armorSprites[(int)packEnemyCard.ArmorType];
        }

        public override void SetViewData(CardView view)
        {
            _background.sprite = view.BackgroundView;
            var enemyCardView = (CreatureCardView)view;

            _itemIcon.sprite = enemyCardView.MainView;
            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + enemyCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять

            _nameBanner.text = enemyCardView.Id;
            _descriptionBanner.text = enemyCardView.Wisecrack;
        }
    }
}