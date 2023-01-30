using Cards;
using UnityEngine;

namespace Controllers
{
    public abstract class LevelManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _container;

        public virtual void ShowEventContainer(LevelCard card)
        {
            _container.SetActive(true);
        }
    }
}