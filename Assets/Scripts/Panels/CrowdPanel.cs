using System.Collections.Generic;
using Definitions.Creatures.Enemies;
using UnityEngine;
using Utils;
using View.EnemyCardView;

namespace Panels
{
    public class CrowdPanel : MonoBehaviour
    {
        [SerializeField] private EnemyCardViewWidget _prefab;
        [SerializeField] private Transform _container;

        [SerializeField] private GameObject _crowdView;
        
        private DataGroup<EnemyCardViewWidget, EnemyPack> _dataGroup;

        private void Awake()
        {
            _dataGroup = new DataGroup<EnemyCardViewWidget, EnemyPack>(_prefab, _container);
        }

        public void Activate(IList<EnemyPack> enemies)
        {
            _dataGroup.SetData(enemies);
            
            _crowdView.SetActive(true);
        }
    }
}