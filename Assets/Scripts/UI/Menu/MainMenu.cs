using Model;
using UnityEngine;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameStarter _gameStarter;

        public void StartGame()
        {
            StartCoroutine(_gameStarter.RunGame());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}