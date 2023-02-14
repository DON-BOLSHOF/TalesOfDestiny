using Cards;
using Panels;
using UnityEngine;

namespace Controllers
{
    public abstract class LevelManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _container;
        [SerializeField] protected AbstractPanelUtil _panelUtil;

        public virtual void ShowEventContainer(LevelCard card)
        {
            _container.SetActive(true);
        }
    }
}