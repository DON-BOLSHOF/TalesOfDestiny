using Controllers;
using LevelManipulation;
using Model.Data;
using Model.Properties;
using UnityEngine;
using Utils;
using Utils.Interfaces;

public class GameSession : MonoBehaviour, IBattleControllerVisitor
{
    [SerializeField] private PlayerData _playerData;

    private EventLevelBoard _eventLevelBoard;

    public GameStateAnalyzer GameStateAnalyzer { get; } =
        new GameStateAnalyzer(GameState.None); //Состояние игры для инвенторя и кэмпа, нельзя будет вызывать кроме None

    public PlayerData Data => _playerData;
    public ObservableProperty<int> LevelTurn { get; } = new ObservableProperty<int>(1);

    private void Awake()
    {
        _eventLevelBoard = FindObjectOfType<EventLevelBoard>();

        _eventLevelBoard.OnNextTurn += OnNextTurn;
    }

    public void Visit(BattleController battleController)
    {
        _playerData.CompanionsData.ReloadCompanions(battleController.Companions);
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