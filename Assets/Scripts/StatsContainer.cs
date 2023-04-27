using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

public class StatsContainer : MonoBehaviour
{
    [SerializeField, ListDrawerSettings(HideAddButton = true)]
    private List<StatAttribute> _StatList = new List<StatAttribute>();

    [Header("Add Stat"), InfoBox("$_AddErrorMessage", InfoMessageType.Error, "_AddError"), PropertyOrder(0)]
    [ValueDropdown("GetAllAttributes"), SerializeField] private string _Attribute;

    [SerializeField, PropertyOrder(1)] private float _Value;

    private string _AddErrorMessage = "";
    private string _NewAttributeErrorMessage = "";
    private bool _AddError;
    private bool _NewAttributeError;
    
    private List<string> GetAllAttributes => StatAttributes.instance.AllAttributes;

    [Button("Add"), PropertyOrder(2)]
    private void AddAttribute()
    {
        if (String.IsNullOrEmpty(_Attribute))
        {
            _AddErrorMessage = "You must select an Attribute to assign the value.";
            _AddError = true;
        }
        else if (_StatList.Any(x => x.GetType() == _Attribute))
        {
            _AddErrorMessage = "Attribute already defined.";
            _AddError = true;
        }
        else
        {
            _StatList.Add(new StatAttribute(_Attribute, _Value));
            _AddError = false;
        }
    }

    [Header("Add new attribute"), InfoBox("$_NewAttributeErrorMessage", InfoMessageType.Error, "_NewAttributeError"), PropertyOrder(3)]
    [SerializeField] private string _AttributeName;

    [Button("Add to attribute list"), PropertyOrder(4)]
    private void AddNewAttribute()
    {
        if (String.IsNullOrEmpty(_AttributeName))
        {
            _NewAttributeErrorMessage = "Attribute cannot be nameless.";
            _NewAttributeError = true;
        }
        else if (!StatAttributes.instance.AddAttribute(_AttributeName))
        {
            _NewAttributeErrorMessage = "Attribute name is a duplicate.";
            _NewAttributeError = true;
        }
        else
        {
            _AttributeName = "";
            _NewAttributeError = false;
        }
    }
}

[Serializable]
public class StatAttribute
{
    [Title("$GetType")]
    [SerializeField] private float _Value;
    
    [SerializeField, HideInInspector] private int _Type;
    public float Value => _Value;
    
    public StatAttribute( string type, float value)
    {
        _Type = StatAttributes.instance.AllAttributes.IndexOf(type);
        _Value = value;
    }

    public string GetType()
    {
        if (_Type >= 0 && _Type < StatAttributes.instance.AllAttributes.Count())
        {
            return StatAttributes.instance.AllAttributes[_Type];
        }
        else
        {
            return $"Error: Attribute #{_Type} missing.";
        }

    }
}