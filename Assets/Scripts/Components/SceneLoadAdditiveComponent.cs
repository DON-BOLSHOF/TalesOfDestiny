using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class SceneLoadAdditiveComponent : MonoBehaviour
    {
        [SerializeField] private string[] _sceneNames;

        private void Awake()
        {
            foreach (var sceneName in _sceneNames)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
    }
}