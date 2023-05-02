using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotDiscard : InventorySlot
{
    protected override void OnItemSettled()
    {
        base.OnItemSettled();
        DiscardItem();
    }
}
