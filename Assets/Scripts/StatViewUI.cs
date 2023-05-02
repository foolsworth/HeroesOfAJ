using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatViewUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatDetailUI _StatDetailPrefab;
    [SerializeField] private GameObject _StatDetailHeader;
    [SerializeField] private float _RevealDelay = 0.1f;

    private List<StatDetailUI> _DetailedStats = new List<StatDetailUI>();
    private int _StatIndex = 0;
    private Coroutine _ShowingDetails = null;
    private bool _DetailsVisible;

    public void UpdateDetailedStats(Stats characterStats, Stats LHItemStats, Stats RHItemStats)
    {
        if (_ShowingDetails != null)
        {
            StopCoroutine(_ShowingDetails);
        }

        _ShowingDetails = StartCoroutine(TriggerDetails(false));

        var processedAttributes = new List<string>();
        _StatIndex = 0;

        var allStats = new List<Stats>()
        {
            characterStats,
            LHItemStats,
            RHItemStats
        };

        for (int i = 0; i < allStats.Count; i++)
        {
            var stats = allStats[i];

            if (stats == null)
            {
                continue;
            }

            foreach (var stat in stats.StatList)
            {
                if (processedAttributes.Contains(stat.GetType()))
                {
                    return;
                }

                var UIItem = _StatIndex < _DetailedStats.Count
                    ? _DetailedStats[_StatIndex]
                    : Instantiate(_StatDetailPrefab, transform);
                UIItem.gameObject.SetActive(_DetailsVisible);

                UIItem.Initialize(stat.GetType(),
                    i == 0 ? stat.Value : 0f,
                    i switch
                    {
                        0 => LHItemStats != null ? LHItemStats.GetStat(stat.GetType())?.Value ?? 0f : 0f,
                        1 => stat.Value,
                        _ => 0f
                    },
                    i == 2 ? stat.Value : RHItemStats != null ? RHItemStats.GetStat(stat.GetType())?.Value ?? 0f : 0f);

                _DetailedStats.Add(UIItem);
                processedAttributes.Add(stat.GetType());
                _StatIndex++;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("PointerEntered");
        if (_ShowingDetails != null)
        {
            StopCoroutine(_ShowingDetails);
        }

        _ShowingDetails = StartCoroutine(TriggerDetails());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExited");
        if (_ShowingDetails != null)
        {
            StopCoroutine(_ShowingDetails);
        }

        _ShowingDetails = StartCoroutine(TriggerDetails(false));
    }

    private IEnumerator TriggerDetails(bool show = true)
    {
        _DetailsVisible = show;
        if (show && !_StatDetailHeader.activeSelf)
        {
            _StatDetailHeader.SetActive(show);
            yield return new WaitForSeconds(_RevealDelay);
        }

        for (int i = 0; i < _StatIndex; i++)
        {
            var detailedStat = _DetailedStats[show ? i : _StatIndex - 1 - i];
            if (detailedStat.gameObject.activeSelf == show)
            {
                continue;
            }

            detailedStat.gameObject.SetActive(show);
            yield return new WaitForSeconds(_RevealDelay);
        }

        if (!show)
        {
            _StatDetailHeader.SetActive(show);
        }
    }
}