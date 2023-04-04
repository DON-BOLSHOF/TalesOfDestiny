using System;
using System.Collections.Generic;
using Definitions.Creatures.Company;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class HeroCompanionsData
    {
        [SerializeField] private List<CompanionPack> _companionPacks;

        public List<CompanionPack> Companions => _companionPacks;

        public void ReloadCompanions(List<CompanionPack> packs)
        {
            _companionPacks = packs;
        }
    }
}