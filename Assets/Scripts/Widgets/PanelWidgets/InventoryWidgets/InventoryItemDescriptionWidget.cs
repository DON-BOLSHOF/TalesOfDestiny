using System;
using System.Collections;
using System.Collections.Generic;
using Cards.SituationCards.Event.ArmyEvents;
using Definitions.Inventory;
using Model.Data.StorageData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Disposables;

namespace Widgets.PanelWidgets.InventoryWidgets
{
    public class InventoryItemDescriptionWidget : PopUpHint
    {
        [SerializeField] private TextMeshPro _id;
        [SerializeField] private TextMeshPro _description;

        private event Action OnThrownOut;
        private event Action OnUsed;

        public override void Show()
        {
            gameObject.SetActive(true);
            StartRoutine(Show(_graphics), ref _graphicRoutine);
        }
        
        public void OnThrowOutButtonClicked()
        {
            OnThrownOut?.Invoke();
        }

        public void OnUseButtonClicked()
        {
            OnUsed?.Invoke();
        }

        public void SetData(InventoryItem item)
        {
            _id.text = item.Id;
            _description.text = ConverterUtils.ConvertBuffsToString(item.Buffs);
        }
        
        public override void ForceExit()
        {
            _timer?.StopTimer();

            ChangeGraphicsAlphaTo(0);

            gameObject.SetActive(false);
        }

        protected override IEnumerator Disappear(Graphic[] graphics)
        {
            yield return base.Disappear(graphics);
            gameObject.SetActive(false);
        }

        public IDisposable SubscribeOnThrown(Action action)
        {
            OnThrownOut += action;
            return new ActionDisposable(() => OnThrownOut -= action);
        }

        public IDisposable SubscribeOnUsed(Action action)
        {
            OnUsed += action;
            return new ActionDisposable(() => OnUsed -= action);
        }
    }
}