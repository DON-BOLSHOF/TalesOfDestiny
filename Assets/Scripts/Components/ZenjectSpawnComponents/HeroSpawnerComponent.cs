using UnityEngine;
using Utils.Interfaces;
using Zenject;

namespace Components.ZenjectSpawnComponents
{
    public class HeroSpawnerComponent : MonoBehaviour, IZenjectSpawner<HeroBehaviour>
    {
        [Inject] public ZenjectDynamicObject<HeroBehaviour>.Factory _factory { get; set; }

        private void Start()
        {
            var instance = Spawn();
            instance.transform.parent = transform;
        }

        public HeroBehaviour Spawn()
        {
            return _factory.Create();
        }
    }
}