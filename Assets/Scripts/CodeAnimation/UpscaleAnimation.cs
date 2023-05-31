using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeAnimation
{
    public static class UpscaleAnimation
    {
        public static IEnumerator ScaleGraphicElement(Graphic element, float baseValue, float toValue, Action onScaled = null)
        {
            yield return UpscaleImage(element, baseValue, toValue);
            
            onScaled?.Invoke();
            
            yield return DownScaleImage(element, toValue, baseValue);
        }
        
        private static IEnumerator UpscaleImage(Graphic image, float baseValue, float toValue)
        {
            for (var i = baseValue; i <= toValue; i = (float)(i + 0.05))
            {
                image.gameObject.transform.localScale = new Vector3(i, i, 1);
                yield return new WaitForSeconds(0.07f);
            }
            
            image.gameObject.transform.localScale = new Vector3(toValue, toValue, 1);
        }
        
        private static IEnumerator DownScaleImage(Graphic image, float baseValue, float toValue)
        {
            for (var i = baseValue; i >= toValue; i = (float)(i - 0.05))
            {
                image.gameObject.transform.localScale = new Vector3(i, i, 1);
                yield return new WaitForSeconds(0.07f);
            }

            image.gameObject.transform.localScale = new Vector3(toValue, toValue, 1);
        }
    }
}