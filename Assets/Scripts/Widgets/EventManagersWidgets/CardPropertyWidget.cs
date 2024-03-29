﻿using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataGroups;

namespace Widgets.EventManagersWidgets
{
    public class CardPropertyWidget : MonoBehaviour, IItemInstance<Sprite>
    {
        [SerializeField] private Image _image; 
        
        public void SetData(Sprite pack)
        {
            _image.sprite = pack;
        }
    }
}