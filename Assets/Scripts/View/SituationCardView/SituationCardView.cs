﻿using UnityEngine;

namespace View.SituationCardView
{
    [CreateAssetMenu(menuName = "CardView/SituationCardView", fileName = "SituationCardView")]
    public class SituationCardView : PortraitCardView
    {
        [SerializeField] private Sprite[] _propertyIcons;
        [SerializeField] private string _wisecrack;

        public Sprite[] PropertyIcons => _propertyIcons;
        public string Wisecrack => _wisecrack;
    }
}