using UnityEngine;

namespace View.EnemyView
{
    [CreateAssetMenu(menuName = "CardView/EnemyView", fileName = "EnemyView")]
    public class EnemyCardView : CardView
    {
        [SerializeField] private Sprite _mainView;
        [SerializeField] private Vector3 _iconOffset;
        [SerializeField] private string _enemyDescription;

        public Sprite MainView => _mainView;
        public Vector3 IconOffset => _iconOffset;
        public string EnemyDescription => _enemyDescription;
    }
}