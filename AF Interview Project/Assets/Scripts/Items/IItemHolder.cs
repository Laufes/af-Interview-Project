namespace AFSInterview.Items
{
    using UnityEngine;

    public interface IItemHolder
    {
        Item GetItem(bool disposeHolder);
        void SetPosition(Vector3 position);
        void Show();
        ItemType GetItemType();
    }
}
