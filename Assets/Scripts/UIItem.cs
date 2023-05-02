using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _Icon;
    [SerializeField] private  BoxCollider2D _Collider; 
    [SerializeField, ReadOnly] private InventoryItemData _ItemData;
    [SerializeField, ReadOnly] private Stats _ItemStats;
    
    public Stats ItemStats => _ItemStats;
    public InventoryItemData ItemData => _ItemData;

    private Inventory _Inventory;
    private InventorySlot _Slot;

    
    public void SetInteractable(bool interactable)
    {
        _Collider.enabled = interactable;
    }
    
    public void SetItem(CollectableItem collectableItem, Inventory inventory, InventorySlot slot)
    {
        _Slot = slot;
        _Inventory = inventory;
        _ItemData = collectableItem.ItemData;
        _ItemStats = new Stats();
        _Icon.sprite = _ItemData.UIIcon;
        foreach (var stat in _ItemData.Stats.StatList)
        {
            _ItemStats.StatList.Add(new StatAttribute(stat.GetType(), stat.Value));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("I was clicked");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("OnBeginDrag");
        _Slot.SetSlotItem(null);
        transform.SetParent(transform.parent.parent.parent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("OnDrag");
        var screenPoint = Input.mousePosition;
        screenPoint.z = 1f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("OnEndDrag");
        var closestSlot = _Inventory.GetClosestSlot(transform.position);

        if (!closestSlot.Occupied)
        {
            _Slot = closestSlot;
        }

        transform.SetParent(_Slot.transform, true);
        _Slot.SetSlotItem(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerDown");
        _Slot.SetSlotItem(null);
        transform.SetParent(transform.parent.parent.parent, true);
    }
}