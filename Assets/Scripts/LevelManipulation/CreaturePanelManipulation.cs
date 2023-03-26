using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeAnimation;
using UnityEngine;

namespace LevelManipulation
{
    [Serializable]
    public class CreaturePanelManipulation
    {
        private List<Vector3> _creatureBasePositions;

        public CreaturePanelManipulation(List<CreatureBehaviour> creaturesBehaviours)
        {
            _creatureBasePositions =
                new List<Vector3>(Array.ConvertAll(creaturesBehaviours.ToArray(), input => input.transform.position));
        }

        public void ToBasePositions(List<CreatureBehaviour> creaturesBehaviours)
        {
            for (var i = 0; i < creaturesBehaviours.Count; i++)
            {
                creaturesBehaviours[i].gameObject.transform.position = _creatureBasePositions[i];
            }
        }

        public async Task TakeNeighborPosition(CreatureBehaviour creatureFrom, int toCreatureBasePosition)//Херня какая-то
        {
            await CreatureAnimations.Move(creatureFrom,
                creatureFrom.transform.position,
                _creatureBasePositions[toCreatureBasePosition]);
        }
    }
}