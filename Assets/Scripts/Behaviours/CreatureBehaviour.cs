using System;
using System.Threading.Tasks;
using CodeAnimation;
using Components;
using Components.Audio;
using Definitions.Creatures;
using UnityEngine;
using Utils;
using Utils.Disposables;
using View.CreatureCardView;

namespace Behaviours
{
    [RequireComponent(typeof(PlaySoundComponent))]
    public class CreatureBehaviour : MonoBehaviour
    {
        [SerializeField] private CreatureViewWidget _creatureViewWidget; //View
        [SerializeField] private float _deathDelay = 0.4f;

        public CreaturePack CreaturePack { get; private set; } //Data
        private HealthComponent _healthComponent;

        private PlaySoundComponent _sound; // SubClasses

        private readonly DisposeHolder _trash = new DisposeHolder();

        public event Action<CreatureBehaviour> OnDying;

        private void Awake()
        {
            _sound = GetComponent<PlaySoundComponent>();

            _healthComponent = new HealthComponent();

            _trash.Retain(_healthComponent.Health.Subscribe(_creatureViewWidget.ModifyHealth));
            _trash.Retain(_healthComponent.Health.Subscribe(OnDamaged));
            _trash.Retain(new Func<IDisposable>(() =>
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
            _healthComponent.SetBaseValue(CreaturePack.CreatureCard.Health * CreaturePack.Count);
        }

        public void Deactivate()
        {
            _creatureViewWidget.SetState(CreatureViewWidgetStates.Deactivating);
        }

        public async Task Attack(CreatureBehaviour creature)
        {
            var basePosition = transform.position;

            await CreatureAnimations.ScaleElement(gameObject, 1f, 1.15f);
            await CreatureAnimations.Move(this, basePosition, creature.transform.position, 0.7f);

            var pack = UnityUtils.Clone(creature.CreaturePack);//!Нужно тк creature изменить свой CreaturePack!
            creature.Visit(CreaturePack);
            Visit(pack);

            await CreatureAnimations.Move(this, transform.position, basePosition, 1);
            await CreatureAnimations.ScaleElement(gameObject, 1.15f, 1f);
        }

        private void Visit(CreaturePack creaturePack)
        {
            _healthComponent.TakeDamage(creaturePack.CreatureCard.Attack * creaturePack.Count,
                creaturePack.CreatureCard.AttackType, CreaturePack.CreatureCard.ArmorType);
        }

        private void OnDamaged(int newHealthValue)
        {
            _creatureViewWidget.SetState(CreatureViewWidgetStates.Damaged);
            _sound.PlayClip("Harassing");

            CheckAmount(newHealthValue);
            CheckDeath(newHealthValue);
        }

        private void CheckAmount(int newHealthValue)
        {
            var newCount = newHealthValue > 0 ? (newHealthValue - 1) / CreaturePack.CreatureCard.Health + 1 : 0;
            if (CreaturePack.Count == newCount) return;

            _creatureViewWidget.AmountDecrease(newCount, newCount * CreaturePack.CreatureCard.Attack);
            CreaturePack.ModifyCount(-(CreaturePack.Count - newCount));
        }

        private void CheckDeath(int newHealthValue)
        {
            if (newHealthValue > 0) return;

            _creatureViewWidget.SetState(CreatureViewWidgetStates.Dying);
        }

        private async void OnDeath() //В аниматоре
        {
            _sound.PlayClip("Death");

            await Task.Delay((int)(_deathDelay * 100));

            OnDying?.Invoke(this);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}