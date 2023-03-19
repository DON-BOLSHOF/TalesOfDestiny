using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Definitions.Creatures;
using UnityEngine;
using Utils;

namespace Panels
{
    public class CreaturePanel : MonoBehaviour
    {
        [SerializeField] private CreatureBehaviour[] creaturesBehaviours;

        private List<CreaturePack> _creaturePacks;

        private int[] _spawnIndexArray;//В хаотичном порядке заспавнятся, сначала генерится их индексы.

        public CreatureBehaviour RandomCreature =>
            creaturesBehaviours[Random.Range(0, creaturesBehaviours.Length)];

        private void Start()
        {
            _spawnIndexArray = new int[creaturesBehaviours.Length];
            for (var i = 0; i < _spawnIndexArray.Length; i++) _spawnIndexArray[i] = i;
        }

        public void Show()
        {
            MathUtils.SnuffleArray(_spawnIndexArray);
            ActivateCreatures();
        }

        private async void ActivateCreatures()
        {
            for (var i = 0; i < _creaturePacks.Count; i++)
            {
                creaturesBehaviours[_spawnIndexArray[i]].Activate(_creaturePacks[i]);
                await Task.Delay(50);
            }
        }

        public void DynamicInitialization(IEnumerable<CreaturePack> creatures)
        {
            _creaturePacks = creatures.ToList();
        }

        public void Attack(CreatureBehaviour creature)
        {
            RandomCreature.Attack(creature);
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}