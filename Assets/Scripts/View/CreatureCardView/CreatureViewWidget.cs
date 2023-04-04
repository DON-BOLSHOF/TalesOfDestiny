using System;
using System.Collections.Generic;
using CodeAnimation;
using Definitions.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;

namespace View.CreatureCardView
{
    [RequireComponent(typeof(Animator))]
    public class CreatureViewWidget : CardViewWidget
    {
        [Header("Animator")] [SerializeField] private Animator _animator;

        [Space] [Header("OffSet")] 
        [SerializeField] private float _widgetOffsetCorrection;

        [Space] [Header("Texts")] 
        [SerializeField] private TextMeshPro _amountValue;
        [SerializeField] private TextMeshPro _attackValue;
        [SerializeField] private TextMeshPro _healthValue;

        [Space] [Header("Icons")] [SerializeField]
        private Image _itemIcon;

        [SerializeField] private Image _attackIcon;
        [SerializeField] private Image _amountIcon;

        [Space] [Header("Types")] 
        [SerializeField] private Image _armorType;

        [Space] [Header("Sprites")]
        [SerializeField] private Sprite[] _armorSprites;

        [Space] [Header("Colors")] 
        [SerializeField] private Color[] _attackColors;

        public event Action OnDeathAnimation;

        private static readonly int DamagedKey = Animator.StringToHash("Damaged");
        private static readonly int DieKey = Animator.StringToHash("Die");
        private static readonly int DeactivateKey = Animator.StringToHash("Deactivate");

        public void SetData(CreaturePack pack)
        {
            var packEnemyCard = pack.CreatureCard;
            SetViewData(packEnemyCard.View);

            _amountValue.text = pack.Count.ToString();
            _attackValue.text = (pack.Count * packEnemyCard.Attack).ToString();
            _healthValue.text = (pack.Count * packEnemyCard.Health).ToString();

            _armorType.sprite = _armorSprites[(int)packEnemyCard.ArmorType];
            _attackIcon.color = _attackColors[(int)packEnemyCard.AttackType];
        }

        public override void SetViewData(CardView view)
        {
            var enemyCardView = (CreaturesCardView.CreatureCardView)view;

            _itemIcon.sprite = enemyCardView.MainView;
            _itemIcon.rectTransform.anchoredPosition =
                Vector3.zero +
                enemyCardView.IconOffset *
                _widgetOffsetCorrection; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
        }

        public void SetState(CreatureViewWidgetStates state)
        {
            switch (state)
            {
                case CreatureViewWidgetStates.Damaged:
                    _animator.SetTrigger(DamagedKey);
                    break;
                case CreatureViewWidgetStates.Dying:
                    _animator.SetBool(DieKey, true);
                    break;
                case CreatureViewWidgetStates.Deactivating:
                    _animator.SetTrigger(DeactivateKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, "Undefined state");
            }
        }

        public void ModifyHealth(int value)
        {
            _healthValue.text = value.ToString();
        }

        public async void AmountDecrease(int newValue, int newAttackValue)
        {
            _amountValue.text = newValue.ToString();
            _attackValue.text = newAttackValue.ToString();

            var scaler = new List<Task>
            {
                CreatureAnimations.ScaleElement(_amountIcon.gameObject, 1f, 1.5f),
                CreatureAnimations.ScaleElement(_attackIcon.gameObject, 1f, 1.5f)
            };
            await Task.WhenAll(scaler);
            
            scaler = new List<Task>
            {
                CreatureAnimations.ScaleElement(_amountIcon.gameObject, 1.5f, 1f),
                CreatureAnimations.ScaleElement(_attackIcon.gameObject, 1.5f, 1f)
            };
            await Task.WhenAll(scaler);
        }

        private void OnDying() //Аниматор
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
    Dying,
    Deactivating
}