using UnityEngine;
using UnityEngine.Events;

public class InventorySlotEquipped : InventorySlot
{
    [HideInInspector] public UnityEvent EquipmentChanged = new UnityEvent();

    protected override void OnItemSet()
    {
        base.OnItemSet();
        EquipmentChanged.Invoke();
    }

    protected override void OnItemUnSet()
    {
        base.OnItemUnSet();
        EquipmentChanged.Invoke();
    }
}