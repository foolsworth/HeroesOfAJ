using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private StatsContainer _StatsContainer;
        [SerializeField, ReadOnly] private Stats _StatsModifiers = new Stats();
        [SerializeField] private Inventory _Inventory;
        [SerializeField] private List<Transform> _Hands;

        private float _CurrentHealth;

        private void Start()
        {
            UpdateStats();
            _CurrentHealth = GetStat("Health");
        }

        public void UpdateStats()
        {
            _StatsModifiers = new Stats();
            for (int i = 0; i < _Inventory.EquippedSlots.Count; i++)
            {
                var slot = _Inventory.EquippedSlots[i];
                if (slot != null && slot.Occupied)
                {
                    if (_StatsModifiers.StatList.Count == 0)
                    {
                        foreach (var stat in slot.ItemInSlot.ItemStats.StatList)
                        {
                            _StatsModifiers.StatList.Add(new StatAttribute(stat.GetType(), stat.Value));   
                        }
                    }
                    else
                    {
                        foreach (var stat in slot.ItemInSlot.ItemStats.StatList)
                        {
                            var presentStat = _StatsModifiers.GetStat(stat.GetType());
                            if (presentStat == null)
                            {
                                _StatsModifiers.StatList.Add(stat);
                            }
                            else
                            {
                                presentStat.Value += stat.Value;
                            }
                        }
                    }
                }
            }
        }

        public float GetStat(string attribute)
        {
            foreach (var stat in _StatsContainer.Stats.StatList)
            {
                if (stat.GetType() == attribute)
                {
                    var modifier = _StatsModifiers.GetStat(stat.GetType());
                    return stat.Value + (modifier?.Value ?? 0f);
                }
            }

            return 0f;
        }
        
        public void PickupItem(CollectableItem item)
        {
            //Transfer item data and create ui
            if (_Inventory.TryAddItem(item))
            {
                Destroy(item.gameObject);   
            }
        }
    }
}