using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class SceneLoaderComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void SceneLoad()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}