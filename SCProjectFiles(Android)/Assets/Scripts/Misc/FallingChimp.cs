﻿using UnityEngine;

public class FallingChimp : MonoBehaviour 
{
	GameManager m_gameManager;

    public static float m_moveAmount = 0.5f;

	void Start() 
	{
	    m_gameManager = FindObjectOfType<GameManager>();
	}

    void CheatDeath()
    {
        m_gameManager.BackToLandLoseMenu();
    }

    public void Move(float amount)
	{
		float xPos = Mathf.Clamp(transform.position.x + amount , -2.26f , 2.38f);
		float yPos = transform.position.y;
		float zPos = transform.position.z;
		transform.position = new Vector3(xPos , yPos , zPos);
	}

    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Spikes"))
        {
            CheatDeath();
        }
    }
}