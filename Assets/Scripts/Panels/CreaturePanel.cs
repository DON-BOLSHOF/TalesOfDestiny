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
        [SerializeField] private CreatureBehaviour[] _creaturesBehaviours;

        private List<CreaturePack> _creaturePacks;

        private int[] _spawnIndexArray;

        private void Start()
        {
            _spawnIndexArray = new int[_creaturesBehaviours.Length];
            for (var i = 0; i < _spawnIndexArray.Length; i++) _spawnIndexArray[i] = i;
        }

        public void Show()
        {
            MathUtils.SnuffleArray(_spawnIndexArray);
            ActivateCreatures(true);
        }

        private async void ActivateCreatures(bool value)
        {
            for (var i = 0; i < _creaturePacks.Count; i++)
            {
                _creaturesBehaviours[_spawnIndexArray[i]].gameObject.SetActive(value);
                _creaturesBehaviours[_spawnIndexArray[i]].Activate(_creaturePacks[i]);
                await Task.Delay((int)50);
            }
        }
        
        public void DynamicInitialization(IEnumerable<CreaturePack> creatures)
        {
            _creaturePacks = creatures.ToList();
        }
        public void Exit()
        {
            throw new System.NotImplementedException();
        }

    }
}