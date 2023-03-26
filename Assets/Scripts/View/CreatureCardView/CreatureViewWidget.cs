using System;
using Definitions.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.CreatureCardView
{
    [RequireComponent(typeof(Animator))]
    public class CreatureViewWidget : CardViewWidget
    {
        [Header("Animator")] 
        [SerializeField] private Animator _animator;

        public event Action OnDeathAnimation;
        
        [Space][Header("OffSet")]
        [SerializeField] private float _widgetOffsetCorrection;

        [Space] [Header("Texts")]
        [SerializeField] private TextMeshPro _amountValue;
        [SerializeField] private TextMeshPro _attackValue;
        [SerializeField] private TextMeshPro _healthValue;

        [Space][Header("Icons")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Image _attackIcon;
        [SerializeField] private Image _healthIcon;

        [Space][Header("Types")]
        [SerializeField] private Image _armorType;

        [Space][Header("Sprites")]
        [SerializeField] private Sprite[] _armorSprites;

        [Space][Header("Colors")]
        [SerializeField] private Color[] _attackColors;
        
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Die = Animator.StringToHash("Die");

        public void SetData(CreaturePack pack)
        {
            var packEnemyCard = pack.CreatureCard;
            SetViewData(packEnemyCard.View);

            _amountValue.text = pack.Count.ToString();
            _attackValue.text = packEnemyCard.Attack.ToString();
            _healthValue.text = packEnemyCard.Health.ToString();

            _armorType.sprite = _armorSprites[(int)packEnemyCard.ArmorType];
            _attackIcon.color = _attackColors[(int)packEnemyCard.AttackType];
        }

        public override void SetViewData(CardView view)
        {
            var enemyCardView = (EnemyCardView.EnemyCardView)view;

            _itemIcon.sprite = enemyCardView.MainView;
            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + enemyCardView.IconOffset * _widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
        }

        public void SetState(CreatureViewWidgetStates state)
        {
            switch (state)
            {
                case CreatureViewWidgetStates.Damaged:
                    _animator.SetTrigger(Damaged);
                    break;
                case CreatureViewWidgetStates.Dying:
                    _animator.SetBool(Die, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, "Undefined state");
            }
        }

        public void ModifyHealth(int value)
        {
            _healthValue.text = value.ToString();
        }

        private void OnDying()//Аниматор
        {
            OnDeathAnimation?.Invoke();
        }

        public void Deactivate() // Для аниматора
        {
            gameObject.SetActive(false);
        }
    }
}

public enum CreatureViewWidgetStates
{
    Damaged,
    Dying
}