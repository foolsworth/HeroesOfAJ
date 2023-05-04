using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroesOfAJ/ItemCompendium", fileName = "ItemCompendium")]
public class ItemCompendium : ScriptableSingleton<ItemCompendium>
{
    [ListDrawerSettings(HideAddButton = true)]
    public List<InventoryItemData> AllItems = new List<InventoryItemData>();

    [Title("Add New Item")]
    [SerializeField] private InventoryItemData _InventoryItemData;

    [Button("Add Item")]
    private void TryAddAttribute()
    {
        AllItems.Add(_InventoryItemData);
        _InventoryItemData = null;
    }
}

[Serializable]
public class InventoryItemData
{
    [SerializeField, OnValueChanged("CheckForDuplicateName"), InfoBox("$_ErrorMessage", InfoMessageType.Warning, "_Error")] 
    private string _Name;

    [SerializeField, Multiline(5)] private string _Description;
    [SerializeField] private Sprite _UIIcon;
    [SerializeField] private GameObject _MeshPrefab;
    [SerializeField] private bool _Consumable;
    [SerializeField] private Stats _Stats;

    private bool _Error;
    private string _ErrorMessage;

    public string Name => _Name;
    public string Description => _Description;
    public Sprite UIIcon => _UIIcon;
    public GameObject MeshPrefab => _MeshPrefab;
    public bool Consumable => _Consumable;
    public Stats Stats => _Stats;

    private void CheckForDuplicateName()
    {
        if (ItemCompendium.Instance != null)
        {
            var duplicates = 0;
            foreach (var item in ItemCompendium.Instance.AllItems)
            {
                if (item._Name == _Name)
                {
                    duplicates++;
                }
            }

            if (duplicates > (ItemCompendium.Instance.AllItems.Contains(this) ? 1 : 0))
            {
                _Error = true;
                _ErrorMessage = "This item name is not unique!";
            }
            else
            {
                _Error = false;
            }
        }
    }
}