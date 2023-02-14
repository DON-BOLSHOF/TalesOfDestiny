using System;
using Panels;
using UnityEngine;

namespace View.EndJourneyCardView
{
    [RequireComponent(typeof(Animator))]
    public class EndJourneyPanelAnimation : MonoBehaviour
    {
        private Animator _animator;
        private EventPanelUtil _util; // Да низкий уровень напрямую знает о более высоком, но я чет подзаколебался прописывать прослойки.
        private static readonly int IsEmerged = Animator.StringToHash("IsEmerged");

        private void Start()
        {
            _util = GetComponentInParent(typeof(EventPanelUtil)) as EventPanelUtil; //Да нарушение DIP-a
            _animator = GetComponent<Animator>();

            if (_util == null) throw new ArgumentException("PanelUtil wasn't found");
            
            _util.DissolveAnimation.OnEmerged += Enlight;
            _util.DissolveAnimation.OnStartDissolving += ToAbsoluteBlack;
        }

        private void ToAbsoluteBlack()
        {
            _animator.SetBool(IsEmerged, false);
        }

        private void Enlight()
        {
            _animator.SetBool(IsEmerged, true);
        }
    }
}