using UnityEngine;

namespace Components
{
    public class DontDestroyOnLoadComponent : MonoBehaviour
    {
        private void Start()
        {
            var goes = FindObjectsOfType<DontDestroyOnLoadComponent>();
            foreach (var go in goes)
            {
                if (go == this) continue;
                
                if (go.name.Equals(gameObject.name))
                    Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}