using Definitions.Enemies;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace View.EnemyCardView
{
    public class EnemyCardViewWidget : ItemWidgetView, IItemInstance<EnemyPack>
    {
        [Header("Icons")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Image _attackType;
        [SerializeField] private Image _armorType;

        [Space][Header("SpriteTypes")]
        [SerializeField] private Sprite[] _attackSprites;
        [SerializeField] private Sprite[] _armorSprites;

        [Space][Header("Banners")]
        [SerializeField] private Text _nameBanner;
        [SerializeField] private Text _descriptionBanner;

        [Space][Header("NumericVariables")]
        [SerializeField] private Text _turnThesholdValue;
        [SerializeField] private Text _enemyAmountValue;
        [SerializeField] private Text _attackValue;
        [SerializeField] private Text _healthValue;
        
        [Space][Header("OffSet")]
        [SerializeField] private float widgetOffsetCorrection;

        public void SetData(EnemyPack pack)
        {
            SetViewData(pack.EnemyCard.View);

            _turnThesholdValue.text = pack.EnemyCard.TurnThreshold.ToString();
            _enemyAmountValue.text = pack.Count.ToString();
            _attackValue.text = pack.EnemyCard.Attack.ToString();
            _healthValue.text = pack.EnemyCard.Health.ToString();

            _attackType.sprite = _attackSprites[(int)pack.EnemyCard.AttackType];
            _armorType.sprite = _armorSprites[(int)pack.EnemyCard.ArmorType];
        }

        public override void SetViewData(CardView view)
        {
            base.SetViewData(view);
            var enemyCardView = (EnemyView.EnemyCardView)view;

            _itemIcon.sprite = enemyCardView.MainView;
            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + enemyCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять

            _nameBanner.text = enemyCardView.Id;
            _descriptionBanner.text = enemyCardView.EnemyDescription;
        }
    }
}