
public class InventorySlotDiscard : InventorySlot
{
    protected override void OnItemSettled()
    {
        base.OnItemSettled();
        DiscardItem();
    }
}
