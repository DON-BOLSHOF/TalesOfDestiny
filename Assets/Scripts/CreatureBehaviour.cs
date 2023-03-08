using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _attack;
    [SerializeField] private int _amount;

    [SerializeField] private Animator _animator;
}