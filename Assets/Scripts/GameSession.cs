using LevelManipulation;
using Model.Data;
using Model.Properties;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private EventLevelBoard _eventLevelBoard;

    public PlayerData Data => _playerData;
    public ObservableProperty<int> LevelTurn { get; } = new ObservableProperty<int>(1);

    private void Awake()
    {
        _eventLevelBoard = FindObjectOfType<EventLevelBoard>();
        
        _eventLevelBoard.OnNextTurn += OnNextTurn;
    }

    private void OnNextTurn()
    {
        LevelTurn.Value++;
    }
    private void OnDestroy()
    {
        _eventLevelBoard.OnNextTurn -= OnNextTurn;
    }
}
