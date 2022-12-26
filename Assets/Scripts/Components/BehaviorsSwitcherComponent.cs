using UnityEngine;

namespace Components
{
    public class BehaviorsSwitcherComponent : MonoBehaviour
    {
        [SerializeField] private Behaviour[] _behaviours;

        public void Switch(bool value)
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.enabled = value;
            }
        }
    }
}