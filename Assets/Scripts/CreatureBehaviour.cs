using CodeAnimation;
using Components;
using Components.Audio;
using Definitions.Creatures;
using UnityEngine;
using Utils.Disposables;
using View.CreatureCardView;

[RequireComponent(typeof(Animator), typeof(PlaySoundComponent))]
public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private CreatureViewWidget _creatureViewWidget;//View
    
    private CreatureCard _creature;//Data
    private HealthComponent _healthComponent;
    
    private Animator _animator;// SubClasses
    private PlaySoundComponent _sound;

    private readonly DisposeHolder _trash = new DisposeHolder();
    
    private static readonly int Damaged = Animator.StringToHash("Damaged");
    private static readonly int Die = Animator.StringToHash("Die");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sound = GetComponent<PlaySoundComponent>();
        
        _healthComponent = new HealthComponent();
    }

    public void Activate(CreaturePack creaturePack)
    {
        gameObject.SetActive(true);
        _creatureViewWidget.SetData(creaturePack);
        _creature = creaturePack.CreatureCard;
        _healthComponent.SetValue(_creature.Health);
        
        _trash.Retain(_healthComponent.Health.Subscribe(_creatureViewWidget.ModifyHealth));
        _trash.Retain(_healthComponent.Health.Subscribe(OnDamaged));
    }

    public async void Attack(CreatureBehaviour creature)
    {
        var basePosition = transform.position;

        await CreatureAnimations.ScaleElement(gameObject, 1f, 1.15f);
        await CreatureAnimations.Move(this, basePosition, creature.transform.position, 0.7f);
        creature.Visit(this);
        Visit(creature);
        await CreatureAnimations.Move(this, transform.position, basePosition, 1);
        await CreatureAnimations.ScaleElement(gameObject, 1.15f, 1f);
    }

    private void Visit(CreatureBehaviour creatureBehaviour)
    {
        _healthComponent.TakeDamage(creatureBehaviour._creature.Attack, creatureBehaviour._creature.AttackType,
            _creature.ArmorType);
    }
    
    private void OnDamaged(int newHealthValue)
    {
        _animator.SetTrigger(Damaged);
        _sound.PlayClip("Harassing");

        CheckDeath(newHealthValue);
    }

    private void CheckDeath(int newHealthValue)
    {
        if (newHealthValue > 0) return;
        _animator.SetBool(Die, true);
    }

    public void OnDeath()
    {
        _sound.PlayClip("Death");
    }

    private void OnDisable()
    {
        _trash.Dispose();
    }
}