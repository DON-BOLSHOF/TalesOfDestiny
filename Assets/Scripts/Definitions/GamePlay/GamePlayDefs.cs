using UnityEngine;

namespace Definitions.GamePlay
{
    [CreateAssetMenu(menuName = "Defs/GamePlayDefs", fileName = "GamePlayDefs")]
    public class GamePlayDefs : ScriptableObject
    {
        [SerializeField] private Vector2Int _battleHeroPosition;

        public Vector2Int BattleHeroPosition => _battleHeroPosition;
    }
}