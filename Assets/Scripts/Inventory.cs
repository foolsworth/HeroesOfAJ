using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot _SlotPrefab;
    [SerializeField] private GameObject _Equipped;
    [SerializeField] private GameObject _Storage;
    [SerializeField] private int _SlotCount = 15;
    [SerializeField] private int _MaxSlotCount = 32;
    [SerializeField] private UIItem _UIItemPrefab;
    [SerializeField] private InventorySlot _DiscardSlot;

    private List<InventorySlotEquipped> _EquippedSlots = new List<InventorySlotEquipped>();
    private List<InventorySlot> _StorageSlots = new List<InventorySlot>();

    public List<InventorySlotEquipped> EquippedSlots => _EquippedSlots;

    private void Start()
    {
        _StorageSlots = _Storage.GetComponentsInChildren<InventorySlot>().ToList();
        _EquippedSlots = _Equipped.GetComponentsInChildren<InventorySlotEquipped>().ToList();
        UpdateSlots();
    }

    public InventorySlot GetNextAvailableSlot()
    {
        foreach (var slot in _StorageSlots)
        {
            if (!slot.Occupied)
            {
                return slot;
            }
        }

        return null;
    }

    public InventorySlot GetClosestSlot(Vector3 position)
    {
        var allSlots = new List<InventorySlot>();
        allSlots.AddRange(_EquippedSlots);
        allSlots.AddRange(_StorageSlots);
        allSlots.Add(_DiscardSlot);

        var closestSlot = 0;

        for (int i = 0; i < allSlots.Count; i++)
        {
            var slot = allSlots[i];
            if (slot != null && Vector3.Distance(slot.transform.position, position) <
                Vector3.Distance(allSlots[closestSlot].transform.position, position))
            {
                closestSlot = i;
            }
        }

        return allSlots[closestSlot];
    }

    public bool TryAddItem(CollectableItem item)
    {
        var nextAvailableSlot = GetNextAvailableSlot();
        if (nextAvailableSlot != null)
        {
            var itemScreenPos = item.transform.position;
            var uiItem = Instantiate(_UIItemPrefab, nextAvailableSlot.transform, false);
            var uiItemTransform = uiItem.transform;
            uiItemTransform.position = itemScreenPos;
            uiItemTransform.localRotation = Quaternion.identity;
            uiItem.SetItem(item, this, nextAvailableSlot);
            nextAvailableSlot.SetSlotItem(uiItem);
            return true;
        }

        return false;
    }

    public void AddSlots(int amount)
    {
        _SlotCount += Math.Clamp(amount, 0, _MaxSlotCount - _SlotCount);
        UpdateSlots();
    }

    public void RemoveSlots(int amount = 0)
    {
        _SlotCount -= Math.Clamp(amount == 0 ? _SlotCount : 0, 0, _SlotCount);
        UpdateSlots();
    }

    private void UpdateSlots()
    {
        if (_SlotPrefab == null)
        {
            Debug.LogError("Slot Prefab not set");
            return;
        }

        var slotsMissing = _SlotCount - _StorageSlots.Count();

        if (slotsMissing == 0)
        {
            return;
        }

        if (slotsMissing < 0)
        {
            var slotsToRemove = Math.Abs(slotsMissing);
            for (int i = 0; i < slotsToRemove; i++)
            {
                var slot = _StorageSlots[^1];
                slot.DiscardItem();
                Destroy(slot.gameObject);
                _StorageSlots.Remove(slot);
            }
        }
        else
        {
            for (int i = 0; i < slotsMissing; i++)
            {
                var slot = Instantiate(_SlotPrefab, transform);
                _StorageSlots.Add(slot);
            }
        }
    }
}