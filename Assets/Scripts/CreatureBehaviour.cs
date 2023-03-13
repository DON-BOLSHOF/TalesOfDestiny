using Definitions.Creatures;
using UnityEngine;
using View.CreatureCardView;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private CreatureViewWidget creatureViewWidget;
    
    private CreatureCard _creature;

    public void Activate(CreaturePack creaturePack)
    {
        creatureViewWidget.SetData(creaturePack);
        _creature = creaturePack.CreatureCard;
    }

    public void Attack(CreatureBehaviour creature)
    {
    }
}