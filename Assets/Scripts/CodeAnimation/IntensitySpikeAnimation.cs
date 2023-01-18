using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CodeAnimation
{
    [RequireComponent(typeof(Light2D))]
    public class IntensitySpikeAnimation : MonoBehaviour
    {
        private static FieldInfo m_FalloffField =
            typeof(Light2D).GetField("m_FalloffIntensity", BindingFlags.NonPublic | BindingFlags.Instance);
        //Магия отражения вот такая вот у нас. 

        private Light2D _light;

        private void Start()
        {
            _light = GetComponent<Light2D>();

            StartCoroutine(IntensityHigh());
        }

        private IEnumerator IntensityHigh()
        {
            for (float i = 0.5f; i <= 0.725; i += 0.0375f)
            {
                m_FalloffField.SetValue(_light, i);
                yield return new WaitForSeconds(0.14f);
            }

            StartCoroutine(IntensityLow());
        }

        private IEnumerator IntensityLow()
        {
            for (float i = 0.725f; i >= 0.5f; i -= 0.0375f)
            {
                m_FalloffField.SetValue(_light, i);
                yield return new WaitForSeconds(0.14f);
            }

            StartCoroutine(IntensityHigh());
        }
    }
}