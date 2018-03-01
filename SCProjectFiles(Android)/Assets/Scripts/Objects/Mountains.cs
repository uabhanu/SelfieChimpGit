﻿using UnityEngine;

public class Mountains : MonoBehaviour 
{
	LevelCreator m_levelCreator;

	void Start() 
	{
		m_levelCreator = FindObjectOfType<LevelCreator>();
	}

	void Update() 
	{
		if(Time.timeScale == 0f)
		{
			return;
		}

        transform.Translate(Vector2.left * (m_levelCreator.m_gameSpeed / 4) * Time.deltaTime);

		if(transform.position.x <= -43.2f)
		{
			transform.position = new Vector3(0f , transform.position.y , transform.position.z);
		}
	}
}
