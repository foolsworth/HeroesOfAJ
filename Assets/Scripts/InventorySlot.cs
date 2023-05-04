using System.Collections;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private UIItem _ItemInSlot;

    public bool Occupied => _ItemInSlot != null;
    public UIItem ItemInSlot => _ItemInSlot;

    protected virtual void OnItemSettled()
    {
        _ItemInSlot.SetInteractable(true);
    }

    protected virtual void OnItemSet()
    {
        _ItemInSlot.SetInteractable(false);
    }

    protected virtual void OnItemUnSet()
    {
    }

    public void SetSlotItem(UIItem item)
    {
        _ItemInSlot = item;
        if (_ItemInSlot != null)
        {
            OnItemSet();
            StartCoroutine(LerpUIItem(item));
        }
        else
        {
            OnItemUnSet();
        }
    }

    public void DiscardItem()
    {
        if (_ItemInSlot != null)
        {
            StopAllCoroutines();
            Destroy(_ItemInSlot.gameObject);
            _ItemInSlot = null;
        }
    }

    private IEnumerator LerpUIItem(UIItem item)
    {
        //Determine position of item
        var targetPosition = Vector3.zero;
        var itemTransform = item.transform;

        //Lerp item position and scale
        while (Vector3.Distance(item.transform.localPosition, targetPosition) > 5f)
        {
            itemTransform.localPosition = Vector3.Lerp(itemTransform.localPosition, targetPosition, Time.deltaTime * 5);
            yield return null;
        }

        itemTransform.localPosition = targetPosition;
        OnItemSettled();
    }
}