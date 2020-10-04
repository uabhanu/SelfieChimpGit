﻿using SelfiePuss.Events;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreManagerSO _scoreManagerSO;
    [SerializeField] private Text _highScoreValueText;

    private void Start()
    {
        _scoreManagerSO.m_ScoreValue = BhanuPrefs.GetHighScore();
        _highScoreValueText.text = _scoreManagerSO.m_ScoreValue.ToString();
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void OnScoreChanged()
    {
        _highScoreValueText.text = _scoreManagerSO.m_ScoreValue.ToString();
    }

    private void RegisterEvents()
    {
        EventsManager.SubscribeToEvent(SelfiePussEvent.ScoreChanged , OnScoreChanged);
    }

    private void UnregisterEvents()
    {
        EventsManager.UnsubscribeFromEvent(SelfiePussEvent.ScoreChanged , OnScoreChanged);
    }
}
