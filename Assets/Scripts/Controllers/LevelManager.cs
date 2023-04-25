using Cards;
using Panels;
using UnityEngine;

namespace Controllers
{
    public abstract class LevelManager : MonoBehaviour
    {
        [SerializeField] protected AbstractTextPanelUtil _textPanelUtil;

        public virtual void ShowEventContainer(LevelCard card)
        {
            _textPanelUtil.gameObject.SetActive(true);
        }
    }
}