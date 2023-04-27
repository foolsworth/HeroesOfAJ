using Sirenix.OdinInspector;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private bool _IsConsumable;
    [SerializeField, ShowIf("_IsConsumable")] private float _EffectDuration;
    
    private StatsContainer _StatsContainer;

    private void Start()
    {
        if (TryGetComponent<StatsContainer>(out var statsContainer))
        {
            _StatsContainer = statsContainer;
        }
    }

    protected virtual void ActivateItem()
    {
        
    }
}
