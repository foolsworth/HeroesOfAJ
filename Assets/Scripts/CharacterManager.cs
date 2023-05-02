using System;
using System.Collections;
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

        [SerializeField] private StatBarController _HealthBar;
        [SerializeField] private StatBarController _ManaBar;
        [SerializeField] private StatBarController _StaminaBar;

        [SerializeField] private StatViewUI _StatView;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _Inventory.EquippedSlots != null && _Inventory.EquippedSlots.Count == 2);
            UpdateStats();
            foreach (var slot in _Inventory.EquippedSlots)
            {
                slot.EquipmentChanged.AddListener(UpdateStats);
            }
        }

        private void OnDestroy()
        {
            foreach (var slot in _Inventory.EquippedSlots)
            {
                slot.EquipmentChanged.RemoveListener(UpdateStats);
            }
        }

        private void UpdateStats()
        {
            UpdateStatModifiers();
            UpdateRealtimeStats();
            if (_StatView != null)
            {
                var rhStats = _Inventory.EquippedSlots[0].ItemInSlot == null
                    ? null
                    : _Inventory.EquippedSlots[0].ItemInSlot.ItemStats;
                var lhStats = _Inventory.EquippedSlots[1].ItemInSlot == null
                    ? null
                    : _Inventory.EquippedSlots[1].ItemInSlot.ItemStats;
                
                _StatView.UpdateDetailedStats(_StatsContainer.Stats, lhStats, rhStats);
            }
        }

        public void UpdateRealtimeStats()
        {
            if (_HealthBar != null)
            {
                _HealthBar.RealtimeStat.UpdateValues(GetModifiedStat("Health"), "Health");
            }

            if (_ManaBar != null)
            {
                _ManaBar.RealtimeStat.UpdateValues(GetModifiedStat("Mana"), "Mana");
            }

            if (_StaminaBar != null)
            {
                _StaminaBar.RealtimeStat.UpdateValues(GetModifiedStat("Stamina"), "Stamina");
            }
        }

        public void UpdateStatModifiers()
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

        public void UpdateBaseStats()
        {
            
        }

        public float GetModifiedStat(string attribute)
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