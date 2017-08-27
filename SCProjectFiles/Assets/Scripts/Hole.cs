﻿//using GooglePlayGames;
using System.Collections;
using UnityEngine;

public class Hole : MonoBehaviour 
{
    BoxCollider2D m_chimpCollider2D , m_holeCollider2D;
	ChimpController m_chimpControlScript;
    float m_startXPos , m_startYPos;
    Ground m_groundScript;
    //int holeAchievementScore;
    Rigidbody2D m_holeBody2D;
    SpriteRenderer m_holeRenderer;
    
    //[SerializeField] string achievementID;

	void Start() 
	{
        m_chimpCollider2D = GameObject.Find("Chimp").GetComponent<BoxCollider2D>();
		m_chimpControlScript = GameObject.Find("Chimp").GetComponent<ChimpController>();
        m_groundScript = FindObjectOfType<Ground>();
        m_holeBody2D = GetComponent<Rigidbody2D>();
        m_holeCollider2D = GetComponent<BoxCollider2D>();
        m_holeRenderer = GetComponent<SpriteRenderer>();
        m_startXPos = transform.position.x;
        m_startYPos = transform.position.y;
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        m_holeBody2D.velocity = new Vector2(-m_groundScript.speed , m_holeBody2D.velocity.y);
    }

    void Update()
    {
        if(Time.timeScale == 0f)
        {
            return;
        }

        if(m_chimpControlScript.m_superMode)
        {
            m_holeCollider2D.enabled = false;
            m_holeRenderer.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col2D)
	{
        if(col2D.gameObject.tag.Equals("Cleaner"))
        {
            transform.position = new Vector2(m_startXPos , m_startYPos);
        }

		if(col2D.gameObject.tag.Equals("Player"))
		{
			Debug.Log("Player in the Hole"); //Working

            //			holeAchievementScore++; //All working fit and fine and beware that the achievement progress won't be updated now as the code is commented

            //			if(Social.localUser.authenticated)
            //			{
            //				PlayGamesPlatform.Instance.IncrementAchievement(achievementID , 1 , (bool success) => //Working and enable this code for final version
            //				{
            //					Debug.Log("Incremental Achievement");
            //				});
            //
            //				if(holeAchievementScore == 50)
            //				{
            //					PlayGamesPlatform.Instance.IncrementAchievement(achievementID , holeAchievementScore , (bool success) => //Working and enable this code for final version
            //					{
            //						Debug.Log("Incremental Achievement");
            //						holeAchievementScore = 0;
            //					});
            //				}
            //			}


            m_chimpControlScript.m_chimpInTheHole = true;
            m_chimpCollider2D.isTrigger = true;
            m_chimpControlScript.StartCoroutine("ChimpCollider2D");
		}
	}
}