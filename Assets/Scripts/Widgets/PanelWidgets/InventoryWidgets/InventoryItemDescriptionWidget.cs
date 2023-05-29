using System.Collections;
using Definitions.Inventory;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Widgets.PanelWidgets.InventoryWidgets
{
    public class InventoryItemDescriptionWidget : PopUpHint
    {
        [SerializeField] private TextMeshPro _id;
        [SerializeField] private TextMeshPro _description;

        [SerializeField] private Button _activateButton;

        public ReactiveEvent OnThrownOut = new ReactiveEvent();
        public ReactiveEvent OnUsed = new ReactiveEvent();

        public override void Show()
        {
            gameObject.SetActive(true);
            StartRoutine(Show(_graphics), ref _graphicRoutine);
        }

        public void OnThrowOutButtonClicked()
        {
            OnThrownOut?.Execute();
        }

        public void OnUseButtonClicked()
        {
            OnUsed?.Execute();
        }

        public void SetData(InventoryItem item)
        {
            _id.text = item.Id;
            _activateButton.gameObject.SetActive(item.ItemState == InventoryItemState.Active ||
                                                 item.ItemState == InventoryItemState.Both);
            _description.text = $"{item.Description}\n{ConverterUtils.ConvertBuffsToString(item.Buffs)}";
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
    }
}