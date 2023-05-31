using Definitions;
using Model.Properties;
using UnityEngine;

namespace Model
{
    public class GlobalHeroMover
    {
        public ObservableProperty<Vector2> GlobalHeroPosition = new ObservableProperty<Vector2>();

        public void ChangeHeroPosition(Vector2 value)
        {
            GlobalHeroPosition.Value = value;
        }

        public void MoveToBattlePosition()
        {
            ChangeHeroPosition(DefsFacade.I.GamePlayDefs.BattleHeroPosition);
        }
    }
}