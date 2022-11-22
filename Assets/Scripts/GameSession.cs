using Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    public PlayerData Data => _playerData; //Check
    
    private void Awake()
    {
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
