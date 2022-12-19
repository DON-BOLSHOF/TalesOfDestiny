using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private List<Timer> _timers;

        public void StartTimer(string name)
        {
            var timer = _timers.Find(timer => timer.Index.Equals(name));

            if (timer == null)
            {
                Debug.LogError($"Not Founded timer with index {name}");
                return;
            }

            StartCoroutine(OnStartTimer(timer));
        }

        private IEnumerator OnStartTimer(Timer timer)
        {
            yield return new WaitForSeconds(timer.Time);
            timer.Event?.Invoke();
        }
    }

    [Serializable]
    class Timer
    {
        [SerializeField] private string _index;
        [SerializeField] private float _time;
        [SerializeField] private UnityEvent _event;

        public string Index => _index;
        public float Time => _time;
        public UnityEvent Event => _event;
    }
}