using Cards;
using Cards.SituationCards;
using UnityEngine;

namespace Controllers
{
    public abstract class BaseController : MonoBehaviour
    {
        [SerializeField] protected GameObject _container;

        public virtual void Show(LevelCard card)
        {
            _container.SetActive(true);
        }
    }
}