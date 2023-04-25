using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Behaviours;
using Definitions.Creatures;
using LevelManipulation;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Random = UnityEngine.Random;

namespace Panels
{
    public class CreaturePanel : MonoBehaviour
    {
        [SerializeField] private List<CreatureBehaviour> _allCreature; //Все бэхи которые могут быть на панели

        private List<CreatureBehaviour> _activeCreatures; //Активные существа
        public int ActiveCreaturesAmount => _activeCreatures.Count;

        private List<CreaturePack> _creaturePacks; //Data

        private int[] _spawnIndexArray; //В хаотичном порядке заспавнятся, сначала генерится их индексы.
        private int _spawnDelay = 50;

        private CreaturePanelManipulation _creaturePanelManipulation;
        private readonly DisposeHolder _trash = new DisposeHolder();

        public event Action OnExit;

        public CreatureBehaviour RandomCreature =>
            _activeCreatures[Random.Range(0, _activeCreatures.Count)];

        private void Awake()
        {
            _creaturePanelManipulation = new CreaturePanelManipulation(_allCreature);

            foreach (var creaturesBehaviour in _allCreature)
            {
                _trash.Retain(new Func<IDisposable>(() =>
                {
                    creaturesBehaviour.OnDying += OnCreatureDeath;
                    return new ActionDisposable(() =>creaturesBehaviour.OnDying -= OnCreatureDeath);
                })());
            }
        }

        public async Task Show()
        {
            await ActivateCreatures();
        }

        public void DynamicInitialization(IEnumerable<CreaturePack> creatures, int spawnDelay)
        {
            _creaturePacks = creatures.ToList();
            _activeCreatures = _allCreature.Take(creatures.Count()).ToList();
            RandomizeSpawnIndexes(creatures.Count());
            _spawnDelay = spawnDelay;
        }

        public async Task ForceToAttack(CreatureBehaviour creature)
        {
            await RandomCreature.Attack(creature);
        }

        private async Task ActivateCreatures()
        {
            _creaturePanelManipulation.ToBasePositions(_activeCreatures);

            for (var i = 0; i < _creaturePacks.Count; i++)
            {
                _activeCreatures[_spawnIndexArray[i]].Activate(_creaturePacks[i]);
                await Task.Delay(_spawnDelay);
            }
        }

        private async void OnCreatureDeath(CreatureBehaviour creature)
        {
            ReloadBehaviours(creature);

            if (_activeCreatures.Count > 0)
                await ReloadPositions();
            else
                Exit();
        }

        private void ReloadBehaviours(CreatureBehaviour creature)
        {
            _creaturePacks.Remove(creature.CreaturePack);
            _activeCreatures.Remove(creature);
        }

        private async Task ReloadPositions()
        {
            var globalPositions = FindGlobalIndexes();

            var tasks = new List<Task>();
            for (var i = 0; i < _activeCreatures.Count; i++)
            {
                if (globalPositions[i] == i) continue;

                if (globalPositions.IndexOf(i + 1) != -1 && globalPositions.IndexOf(i + 2) != -1)
                {
                    tasks.Add(_creaturePanelManipulation.TakeNeighborPosition(_activeCreatures[i + 1],
                        i));
                    globalPositions[i + 1] = i;

                    i = 0; //Заново итерируем
                }
                else
                {
                    tasks.Add(_creaturePanelManipulation.TakeNeighborPosition(_activeCreatures[i],
                        i));
                    globalPositions[i] = i;
                }

                globalPositions.Sort();
            }

            await Task.WhenAll(tasks);
        }

        private List<int> FindGlobalIndexes()
        {
            return _activeCreatures.Select(activeCreature => _allCreature.FindIndex(c => c.Equals(activeCreature)))
                .ToList();
        }

        private void RandomizeSpawnIndexes(int limit)
        {
            if (limit > _allCreature.Count)
                throw new ArgumentException($"Too many indexes, limit: {_allCreature.Count}");

            _spawnIndexArray = new int[limit];
            for (var i = 0; i < _spawnIndexArray.Length; i++) _spawnIndexArray[i] = i;
            MathUtils.SnuffleArray(_spawnIndexArray);
        }

        public List<CreaturePack> TakeActivePacks()
        {
            return _activeCreatures.Select(creature => creature.CreaturePack).ToList();
        }

        private void Exit()
        {
            OnExit?.Invoke();
        }

        public async Task Deactivate()
        {
            foreach (var creature in _allCreature)
            {
                creature.Deactivate();
                await Task.Delay(_spawnDelay);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}