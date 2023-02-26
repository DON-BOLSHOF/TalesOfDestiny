using UnityEngine;
using Utils.Interfaces;

namespace Components
{
    public class SpawnObjectComponent : MonoBehaviour, ISpawner<GameObject>
    {
        [SerializeField] private GameObject _gameObject;

        private void Start()
        {
            Spawn();
        }

        public GameObject Spawn()
        {
            return Instantiate(_gameObject, transform.position, Quaternion.identity);
        }
    }
}
