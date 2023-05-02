using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StatBarController : MonoBehaviour
{
    [SerializeField] private Transform _BarFill;
    [SerializeField] private TextMeshProUGUI _Label;
    [SerializeField] private TextMeshProUGUI _Value;
    [SerializeField] private RealtimeStat _RealtimeStat = new RealtimeStat();

    public RealtimeStat RealtimeStat => _RealtimeStat; 
        
    private void Start()
    {
        _RealtimeStat.StatChanged.AddListener(UpdateBar);
    }

    private void OnDestroy()
    {
        _RealtimeStat.StatChanged.RemoveListener(UpdateBar);
    }

    public void UpdateBar()
    {
        if (_Label != null)
        {
            _Label.text = _RealtimeStat.Name;
        }

        if (_BarFill != null)
        {
            _BarFill.localScale = new Vector3(_RealtimeStat.CurrentValue / _RealtimeStat.MaxValue, 1f, 1f);
        }

        if (_Value != null)
        {
            _Value.text = $"{(int) _RealtimeStat.CurrentValue}/{(int) _RealtimeStat.MaxValue}";
        }
    }
}

[Serializable]
public class RealtimeStat
{
    [Title("$_Name")]
    [ProgressBar(0, "_MaxValue"), SerializeField, ReadOnly]private float _CurrentValue;

    private bool _Initialized;
    private float _MaxValue;
    private string _Name;
    
    [HideInInspector] public UnityEvent StatChanged = new UnityEvent();
    public float CurrentValue => _CurrentValue;
    public float MaxValue => _MaxValue;
    public string Name => _Name;
    

    public void UpdateValues(float maxValue, string name = "")
    {
        _MaxValue = maxValue;
        if (_CurrentValue > _MaxValue)
        {
            _CurrentValue = MaxValue;
        }
        if (!_Initialized)
        {
            _Initialized = true;
            _CurrentValue = _MaxValue;
            _Name = name;
        }
        StatChanged.Invoke();
    }

    public void Heal(float value)
    {
        _CurrentValue += value;
        if (_CurrentValue > _MaxValue)
        {
            _CurrentValue = _MaxValue;
        }
        StatChanged.Invoke();
    }
    
    public void Damage(float value)
    {
        _CurrentValue -= value;
        if (_CurrentValue < 0)
        {
            _CurrentValue = 0;
        }
        StatChanged.Invoke();
    }
}
