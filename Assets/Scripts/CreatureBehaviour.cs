using System;
using System.Threading.Tasks;
using CodeAnimation;
using Components;
using Components.Audio;
using Definitions.Creatures;
using UnityEngine;
using Utils.Disposables;
using View.CreatureCardView;

[RequireComponent(typeof(PlaySoundComponent))]
public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private CreatureViewWidget _creatureViewWidget; //View

    public CreaturePack CreaturePack { get; private set; } //Data
    private HealthComponent _healthComponent;

    private PlaySoundComponent _sound;// SubClasses

    private readonly DisposeHolder _trash = new DisposeHolder();

    private Action<CreatureBehaviour> _onDying;

    private void Awake()
    {
        _sound = GetComponent<PlaySoundComponent>();

        _healthComponent = new HealthComponent();
        
        _trash.Retain(_healthComponent.Health.Subscribe(_creatureViewWidget.ModifyHealth));
        _trash.Retain(_healthComponent.Health.Subscribe(OnDamaged));
        _trash.Retain(new Func<IDisposable>(()=>
        {
            _creatureViewWidget.OnDeathAnimation += OnDeath;
            return new ActionDisposable(() => _creatureViewWidget.OnDeathAnimation -= OnDeath);
        })());
    }

    public void Activate(CreaturePack creaturePack)
    {
        _creatureViewWidget.gameObject.SetActive(true);
        CreaturePack = creaturePack;
        _creatureViewWidget.SetData(CreaturePack);
        _healthComponent.SetBaseValue(CreaturePack.CreatureCard.Health);
    }

    public void Deactivate()
    {
        _creatureViewWidget.Deactivate();
    }

    public async Task Attack(CreatureBehaviour creature)
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
        _healthComponent.TakeDamage(creatureBehaviour.CreaturePack.CreatureCard.Attack, creatureBehaviour.CreaturePack.CreatureCard.AttackType,
            CreaturePack.CreatureCard.ArmorType);
    }

    private void OnDamaged(int newHealthValue)
    {
        _creatureViewWidget.SetState(CreatureViewWidgetStates.Damaged);
        _sound.PlayClip("Harassing");

        CheckDeath(newHealthValue);
    }

    private void CheckDeath(int newHealthValue)
    {
        if (newHealthValue > 0) return;
        
        _creatureViewWidget.SetState(CreatureViewWidgetStates.Dying);
    }

    private void OnDeath()//В аниматоре
    {
        _sound.PlayClip("Death");
        _onDying?.Invoke(this);
    }

    public IDisposable SubscribeDying(Action<CreatureBehaviour> call)
    {
        _onDying += call;
        return new ActionDisposable(() => _onDying -= call);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}