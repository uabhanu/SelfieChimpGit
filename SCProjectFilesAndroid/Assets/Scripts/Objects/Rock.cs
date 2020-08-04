﻿using UnityEngine;

public class Rock : MonoBehaviour 
{
    private GameManager _gameManager;
    private SoundManager m_soundManager;
    
    [SerializeField] private GameObject m_explosionPrefab;

	void Start() 
	{
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
		
    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Player"))
        {
            m_soundManager.m_soundsSource.clip = m_soundManager.m_rockExplosion;
			
            if(m_soundManager.m_soundsSource.enabled)
            {
                m_soundManager.m_soundsSource.Play();
            }

            SpawnExplosion();
        }
    }

	void SpawnExplosion()
	{
        Explosion.m_explosionType = "Rock";
        Instantiate(m_explosionPrefab , transform.position , Quaternion.identity);
		ScoreManager.m_scoreValue += 100;
        _gameManager.m_HighScoreValueText.text = ScoreManager.m_scoreValue.ToString();
		BhanuPrefs.SetHighScore(ScoreManager.m_scoreValue);
        Destroy(gameObject); //TODO Object Pooling instead of Destroy
	}
}
