using UI;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Zenject;

namespace Widgets
{
    [RequireComponent(typeof(PopUpHint))]
    public class GameStageWidget : MonoBehaviour
    {
        [Inject] private GameSession _gameSession;
        
        private PopUpHint _blockHint;

        private readonly DisposeHolder _trash = new DisposeHolder();
        
        private void Awake()
        {
            _blockHint = GetComponent<PopUpHint>();

            _trash.Retain(_gameSession.GameStateAnalyzer.SubscribeOnStateChangeBlocked(ShowHint));
            _trash.Retain(_gameSession.GameStateAnalyzer.GameState.Subscribe(ForceBlockHintExit));
        }
        
        private void ShowHint()
        {
            _blockHint.Show();
        }

        private void ForceBlockHintExit(GameState state)
        {
            if (state == GameState.None && _blockHint.IsTimerRunning) _blockHint.ForceExit();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}