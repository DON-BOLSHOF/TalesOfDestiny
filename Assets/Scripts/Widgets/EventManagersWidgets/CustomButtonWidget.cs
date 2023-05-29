using Model;
using Model.Data.StorageData;
using Panels;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataGroups;
using Zenject;

namespace Widgets.EventManagersWidgets
{ 
    public class CustomButtonWidget : MonoBehaviour, IItemInstance<CustomButton>
    {
        [SerializeField] private Text _text;

        [Inject] private StorageData _storageData;
        
        private AbstractTextPanelUtil textPanelUtil;
        private CustomButton _customButton;

        private bool _alreadyClicked;

        private void Start()
        {
            textPanelUtil = GetComponentInParent<AbstractTextPanelUtil>();
        }

        public void SetData(CustomButton button)
        {
            _text.text = button.Name;
            _customButton = button;
        }

        public void OnClick()
        {
            if(_alreadyClicked) return;
            
            OnButtonVisitorClick();
            
            _alreadyClicked = true;
        }

        private void OnButtonVisitorClick()
        {
            _storageData.InteractData(_customButton.ButtonInteraction as IDataInteraction);
            _storageData.InteractData(_customButton.Tribute);
            _storageData.InteractData(_customButton.ButtonInteraction, textPanelUtil);
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