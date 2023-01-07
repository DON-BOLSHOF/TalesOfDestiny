using UnityEngine;

namespace Components
{
    public class SpawnObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;

        private void Start()
        {
            Instantiate(_gameObject, transform.position, Quaternion.identity);
        }
    }
}
