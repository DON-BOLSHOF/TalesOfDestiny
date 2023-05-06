using System;
using UnityEngine;

namespace Model.Data.StorageData
{
    [Serializable]
    public class PropertyData
    {
        [SerializeField] protected int _food;
        [SerializeField] protected int _coins;
        [SerializeField] protected int _prestige;

        public int Food => _food;
        public int Coins => _coins;
        public int Prestige => _prestige;
    }
}