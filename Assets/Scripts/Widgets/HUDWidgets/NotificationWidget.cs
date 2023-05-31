using Model;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets.HUDWidgets
{
    public class NotificationWidget : PopUpHint
    {
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Text _requestText;

        public void SetData(Notification notification)
        {
            _descriptionText.text = notification.Description;
            _requestText.text = notification.RequestDescription;
        }
    }
}