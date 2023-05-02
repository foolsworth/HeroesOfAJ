using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class StatsContainer : MonoBehaviour
{
    public Stats Stats;
}

[Serializable]
public class Stats 
{
    [FormerlySerializedAs("StatList")] [SerializeField, ListDrawerSettings(HideAddButton = true)]
    private List<StatAttribute> _StatList = new List<StatAttribute>();

    [Header("Add Stat"), InfoBox("$_AddErrorMessage", InfoMessageType.Error, "_AddError"), PropertyOrder(0)]
    [ValueDropdown("GetAllAttributes"), SerializeField] private String _Attribute;

    [SerializeField, PropertyOrder(1)] private float _Value;

    private String _AddErrorMessage = "";
    private String _NewAttributeErrorMessage = "";
    private bool _AddError;
    private bool _NewAttributeError;
    
    private List<String> GetAllAttributes => StatAttributes.Instance.AllAttributes;
    public List<StatAttribute> StatList => _StatList;

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
    [SerializeField] private String _AttributeName;

    [Button("Add to attribute list"), PropertyOrder(4)]
    private void AddNewAttribute()
    {
        if (String.IsNullOrEmpty(_AttributeName))
        {
            _NewAttributeErrorMessage = "Attribute cannot be nameless.";
            _NewAttributeError = true;
        }
        else if (!StatAttributes.Instance.AddAttribute(_AttributeName))
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

    public StatAttribute GetStat(string attribute)
    {
        foreach (var stat in _StatList)
        {
            if (stat.GetType() == attribute)
            {
                return stat;
            }   
        }

        return null;
    }
    
}

[Serializable]
public class StatAttribute
{
    [Title("$GetType")]
    [SerializeField] private float _Value;
    
    [SerializeField, HideInInspector] private int _Type;

    public float Value
    {
        get => _Value;
        set
        {
            _Value = value;
        }
    }
    public int Type => _Type;
    
    public StatAttribute( String type, float value)
    {
        _Type = StatAttributes.Instance.AllAttributes.IndexOf(type);
        _Value = value;
    }

    public String GetType()
    {
        if (StatAttributes.Instance == null)
        {
            return "";
        }
        
        if (_Type >= 0 && _Type < StatAttributes.Instance.AllAttributes.Count())
        {
            return StatAttributes.Instance.AllAttributes[_Type];
        }
        else
        {
            return $"Error: Attribute #{_Type} missing.";
        }

    }
}