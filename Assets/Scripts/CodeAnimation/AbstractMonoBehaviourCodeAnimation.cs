using System.Collections;
using UnityEngine;

namespace CodeAnimation
{
    public abstract class AbstractMonoBehaviourCodeAnimation : MonoBehaviour
    {
        public abstract IEnumerator StartAnimation();
        public abstract IEnumerator EndAnimation();
    }
}