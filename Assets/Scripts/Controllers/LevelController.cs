using Cards;
using UnityEngine;

namespace Controllers
{
    public abstract class LevelController : MonoBehaviour
    {
        [SerializeField] protected GameObject _container;

        public virtual void Show(LevelCard card)
        {
            _container.SetActive(true);
        }
    }
}