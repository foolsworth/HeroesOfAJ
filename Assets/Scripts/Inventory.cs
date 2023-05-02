using System;
using System.Collections;
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

    public List<InventorySlot> EquippedSlots = new List<InventorySlot>();
    private List<InventorySlot> _StorageSlots = new List<InventorySlot>();
    private InventorySlot _DiscardSlot;

    private void Start()
    {
        _StorageSlots = _Storage.GetComponentsInChildren<InventorySlot>().ToList();
        EquippedSlots = _Equipped.GetComponentsInChildren<InventorySlot>().ToList();
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
        allSlots.AddRange(EquippedSlots);
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
        foreach (var slot in _StorageSlots)
        {
            if (!slot.Occupied)
            {
                var itemScreenPos = item.transform.position;
                var uiItem = Instantiate(_UIItemPrefab, slot.transform, false);
                uiItem.transform.position = itemScreenPos;
                uiItem.transform.localRotation = Quaternion.identity;
                uiItem.SetItem(item, this, slot);
                slot.SetSlotItem(uiItem);
                return true;
            }
        }

        return false;
    }
    
    public void AddSlots(int amount)
    {
        _SlotCount +=  Math.Clamp(amount, 0, _MaxSlotCount - _SlotCount);
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
                Destroy(slot);
                _StorageSlots.Remove(slot);
            }
        }else
        {
            for (int i = 0; i < slotsMissing; i++)
            {
                var slot = Instantiate(_SlotPrefab, transform);
                _StorageSlots.Add(slot);
            }
        }
    }
}
