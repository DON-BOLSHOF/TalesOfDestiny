using System.Collections;
using UnityEngine;

namespace CodeAnimation
{
  
    public static class RotateAnimation
    {
        public static IEnumerator Rotate(GameObject gameObject, Quaternion startPosition, Quaternion endPosition, float time)
        {
            var frameTime = time / 100;
            for (float i = 0; i < 1; i += 0.01f)
            {
                gameObject.transform.localRotation = Quaternion.Lerp(startPosition, endPosition, EaseInOutQuint(i));
                yield return new WaitForSeconds(frameTime);
            }
        }

        private static float EaseInOutQuint(float x)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }
    }
}