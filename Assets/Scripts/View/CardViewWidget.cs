using UnityEngine;

namespace View
{
    public abstract class CardViewWidget : MonoBehaviour // Вообще можно абстрактным сделать, но пусть на CardView лучше весит
    {
        public abstract void SetViewData(CardView view);
    }
}