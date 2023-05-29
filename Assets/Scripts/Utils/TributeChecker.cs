using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Data;
using Model.Tributes;

namespace Utils
{
    public static class TributeChecker
    {
        public static bool CheckPropertyFits(ITribute tribute, HeroPropertyData heroPropertyData)
        {
            var propertyData = new
            {
                food = heroPropertyData.Food.Value - tribute.TributePropertyData.Food,
                coins = heroPropertyData.Coins.Value - tribute.TributePropertyData.Coins,
                prestige = heroPropertyData.Prestige.Value - tribute.TributePropertyData.Prestige
            };
            return propertyData.coins >= 0 && propertyData.food >= 0 && propertyData.prestige >= 0;
        }

        public static bool CheckContainingItems(ITribute tribute, HeroInventoryData heroInventoryData)
        {
            var inventoryItems = tribute.TributeInventoryItems;
            return inventoryItems.Select(tributeItem => heroInventoryData.InventoryItems.ToList().
                FindIndex(s => tributeItem.Id.Contains(s.Id))).All(t => t >= 0);
        }

        public static CustomButton[] GetFitsButtons(IEnumerable<CustomButton> buttons, HeroPropertyData propertyData, HeroInventoryData inventoryData)
        {
            var updateButtons = new List<CustomButton>();

            foreach (var customButton in buttons)
            {
                if (customButton.ButtonType == CustomButton.CustomButtonType.TributeButton)
                {
                    if (CheckPropertyFits(customButton.Tribute, propertyData) &&
                        CheckContainingItems(customButton.Tribute, inventoryData))
                        updateButtons.Add(customButton);
                }
                else
                {
                    updateButtons.Add(customButton);
                }
            }

            return updateButtons.ToArray();
        }
    }
}