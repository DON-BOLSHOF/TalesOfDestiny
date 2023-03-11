using System;
using Definitions.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View;
using View.EnemyCardView;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private CreatureWidget _creatureWidget;
    
    private CreatureCard _creature;

    public void Activate(CreaturePack creaturePack)
    {
        _creatureWidget.SetData(creaturePack);
        _creature = creaturePack.CreatureCard;
    }
}

[Serializable]
public class CreatureWidget//ТЕСТ
{
    [Space][Header("OffSet")]
    [SerializeField] private float widgetOffsetCorrection;

    [Space] [Header("Texts")]
    [SerializeField] private TextMeshPro _enemyAmountValue;
    [SerializeField] private TextMeshPro _attackValue;
    [SerializeField] private TextMeshPro _healthValue;

    [Space][Header("Icons")]
    [SerializeField] protected Image _itemIcon;
    [SerializeField] private Image _attackIcon;

    [Space][Header("Types")]
    [SerializeField] private Image _armorType;

    [Space][Header("Sprites")]
    [SerializeField] private Sprite[] _armorSprites;
    
    [Space][Header("Colors")]
    [SerializeField] private Color[] _attackColors;

    public void SetData(CreaturePack pack)
    {
        var packEnemyCard = pack.CreatureCard;
        SetViewData(packEnemyCard.View);

        _enemyAmountValue.text = pack.Count.ToString();
        _attackValue.text = packEnemyCard.Attack.ToString();
        _healthValue.text = packEnemyCard.Health.ToString();

        _armorType.sprite = _armorSprites[(int)packEnemyCard.ArmorType];
        _attackIcon.color = _attackColors[(int)packEnemyCard.AttackType];
    }

    public void SetViewData(CardView view)
    {
        var enemyCardView = (EnemyCardView)view;

        _itemIcon.sprite = enemyCardView.MainView;
        _itemIcon.rectTransform.anchoredPosition = Vector3.zero + enemyCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
    }
}