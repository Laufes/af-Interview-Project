namespace AFSInterview.Items
{
    using System.Collections.Generic;
    using UnityEngine;

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private int money;

        public int Money => money;
        public int ItemsCount => items.Count;

        public void SellAllItemsUpToValue(int maxValue)
        {
            for (int i = 0; i < items.Count; i++)
            {
                int itemValue = items[i].Value;

                if (itemValue > maxValue)
                {
                    continue;
                }

                money += itemValue;
                items.RemoveAt(i);
                --i;
            }
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }
    }
}