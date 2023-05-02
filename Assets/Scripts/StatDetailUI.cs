using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDetailUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Name;
    [SerializeField] private TextMeshProUGUI _Value;
    [SerializeField] private TextMeshProUGUI _LHMod;
    [SerializeField] private TextMeshProUGUI _RHMod;

    public void Initialize(string name, float value, float lvalue, float rValue)
    {
        if (_Name != null)
        {
            _Name.text = name;
        }

        if (_Value != null && value != 0f)
        {
            _Value.text = value.ToString("0");
        }
        else
        {
            _Value.text = "";
        }

        if (_LHMod != null && lvalue != 0f)
        {
            _LHMod.text = lvalue.ToString("0");
        }
        else
        {
            _LHMod.text = "";
        }


        if (_RHMod != null && rValue != 0f)
        {
            _RHMod.text = rValue.ToString("0");
        }
        else
        {
            _RHMod.text = "";
        }

    }
}
