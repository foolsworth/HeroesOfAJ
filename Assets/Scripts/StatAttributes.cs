using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu (menuName = "HeroesOfAJ/HeroAttributes", fileName = "HeroAttributes")]
public class StatAttributes : ScriptableSingleton<StatAttributes>
{
    [ListDrawerSettings(HideAddButton = true), ValidateInput("ValidateChange", "$_ChangeMessage", InfoMessageType.Error)] 
    public List<String> AllAttributes;
    [SerializeField, InfoBox("$_ErrorMessage", InfoMessageType.Error, "_Error")] private string _NewAttribute;
    
    private String _ChangeMessage = "";
    private String _ErrorMessage = "";
    private bool _Error;
    
    
    [Button("Add Attribute")]
    private void TryAddAttribute()
    {
        if (String.IsNullOrEmpty(_NewAttribute))
        {
            _ErrorMessage = "Attribute cannot be nameless.";
            _Error = true;
        }
        else if (! AddAttribute(_NewAttribute))
        {
            _ErrorMessage = "Attribute name is a duplicate.";
            _Error = true;
        }
        else
        {
            _NewAttribute = "";
            _Error = false;
        }
    }

    public bool ValidateChange()
    {
        if (AllAttributes.Contains(""))
        {
            _ChangeMessage = "Attributes can't be empty.";
            return false;
        } 
        if (AllAttributes.Distinct().Count() == AllAttributes.Count())
        {
            return true;
        }

        _ChangeMessage = "Attributes must be unique.";
        return false;
    }
    
    public bool AddAttribute(String attributeName)
    {
        if (!AllAttributes.Contains(attributeName))
        {
            AllAttributes.Add(attributeName);
            return true;
        }

        return false;
    }
}
