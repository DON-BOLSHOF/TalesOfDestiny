using System;
using System.Collections.Specialized;
using System.Linq;
using Definitions.Inventory;
using Model;
using Utils;

namespace UI
{
    public class NotificationHandler
    {
        public ReactiveEvent<Notification[]> OnNotificationSent = new ReactiveEvent<Notification[]>();

        public void NotifyInventory(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notification[] notifications = null;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    var newItems = e.NewItems.Cast<InventoryItem>().ToArray();
                    notifications = Array.ConvertAll(newItems, t => new Notification(t.Id, "Инвентарь пополнен:"));
                    break;
                }
            }

            if (notifications != null)
                OnNotificationSent?.Execute(notifications);
        }
    }
}