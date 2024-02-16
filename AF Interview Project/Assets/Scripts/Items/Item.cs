namespace AFSInterview.Items
{
    using System;
    using UnityEngine;

    [Serializable]
    public class Item
    {
        [SerializeField] private string name;
        [SerializeField] private int value;
        [SerializeField] private bool consumable;
        [SerializeField] private ItemType itemType;
        [Range(0f, 1f)]
        [SerializeField] private float chanceToSpawnItem;

        public string Name => name;
        public int Value => value;
        public bool Consumable => consumable;
        public ItemType ItemType => itemType;
        public float ChanceToSpawnItem => chanceToSpawnItem;

        public Item(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public void Use()
        {
            Debug.Log("Using" + Name);
        }
    }
}