using System.Threading.Tasks;
using UnityEngine;

namespace CodeAnimation
{
    public static class CreatureAnimations
    {
        public static async Task Move(CreatureBehaviour creature, Vector2 myPos, Vector2 posTo, float motionRatio = 1,
            int delayMilliseconds = 40) //Коэффициент для лерпа, насколько далеко продвинется.
        {
            for (float i = 0; i <= motionRatio; i += 0.05f)
            {
                creature.transform.position = Vector3.Lerp(myPos, posTo, EaseInOutQuart(i));
                await Task.Delay(delayMilliseconds);
            }
        }

        public static float EaseInOutQuart(float x)
        {
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
        }

        public static async Task ScaleElement(GameObject element, float fromScale, float toScale,
            int delayMilliseconds = 20)
        {
            for (float i = 0; i <= 1f; i += 0.05f)
            {
                element.transform.localScale =
                    Vector3.Lerp(new Vector3(fromScale, fromScale, fromScale), new Vector3(toScale, toScale,toScale), i);
                await Task.Delay(delayMilliseconds);
            }
        }
    }
}