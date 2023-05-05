using Cards.SituationCards;
using Cards.SituationCards.Event;
using Model;
using Panels;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Widgets.EventManagersWidgets
{ 
    public class CustomButtonWidget : MonoBehaviour, IItemInstance<CustomButton>
    {
        [SerializeField] private Text _text;

        [Inject] private StorageData _storageData;
        
        private AbstractTextPanelUtil textPanelUtil;
        private ButtonInteraction _interaction;

        private bool _alreadyClicked;

        private void Start()
        {
            textPanelUtil = GetComponentInParent<AbstractTextPanelUtil>();
        }

        public void SetData(CustomButton button)
        {
            _text.text = button.Name;
            _interaction = button.ButtonInteraction;
        }

        public void OnClick()
        {
            if(_alreadyClicked) return;
            
            OnButtonVisitorClick();
            
            _alreadyClicked = true;
        }

        private void OnButtonVisitorClick()
        {
            _storageData.InteractData(_interaction, textPanelUtil);
        }

        public void ActivateButton()
        {
            _alreadyClicked = false;
        }
        private void OnDisable()
        {
            ActivateButton();
        }

    }
}