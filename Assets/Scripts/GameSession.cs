using LevelManipulation;
using Model.Data;
using Model.Properties;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private LevelBoard _levelBoard;

    public PlayerData Data => _playerData;
    public ObservableProperty<int> LevelTurn { get; } = new ObservableProperty<int>(1);

    private void Awake()
    {
        _levelBoard = FindObjectOfType<LevelBoard>();
        
        _levelBoard.OnNextTurn += OnNextTurn;
    }

    private void OnNextTurn()
    {
        LevelTurn.Value++;
    }
    private void OnDestroy()
    {
        _levelBoard.OnNextTurn -= OnNextTurn;
    }
}
