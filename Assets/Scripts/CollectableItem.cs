using System;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _Icon;
    [SerializeField, ValueDropdown("GetAllItems"), OnValueChanged("SetItemData")] private int _Item;
    [SerializeField, ReadOnly] protected InventoryItemData _ItemData;
    
    public InventoryItemData ItemData =>  _ItemData;
    protected Collider _Collider;

    private ValueDropdownList<int> GetAllItems()
    {
        var dropdownList = new ValueDropdownList<int>();
        var items = ItemCompendium.Instance.AllItems;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            dropdownList.Add(item.Name, i);
        }

        return dropdownList;
    }

    private void SetItemData()
    {
        var items = ItemCompendium.Instance.AllItems;
        if (_Item >= 0 && _Item < items.Count)
        {
            _ItemData = items[_Item];
            _Icon.sprite = _ItemData.UIIcon;
        }
        else
        {
            _ItemData = null;
        }
    }

    private void Update()
    {
        if (_Icon != null)
        {
            _Icon.transform.LookAt(Camera.main.transform.position);
        }
    }

    private void Start()
    {
        if (TryGetComponent<Collider>(out var collider))
        {
            _Collider = collider;
        }

        SetItemData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterManager>(out var hero))
        {
            //Get Picked up
            hero.PickupItem(this);
        }
    }

    protected virtual void ActivateItem()
    {
        //Add
    }
}
