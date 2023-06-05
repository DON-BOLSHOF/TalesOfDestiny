using UnityEngine;

namespace UI.Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private float _defaultTimeScale;
        
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");

        public void OpenMenu()
        {
            _animator.SetTrigger(Open);
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public void Continue()
        {
            _animator.SetTrigger(Close);
            
            Time.timeScale = _defaultTimeScale;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}