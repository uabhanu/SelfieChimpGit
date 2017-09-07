﻿using System.Collections;
using UnityEngine;

public class Swipe : MonoBehaviour 
{
	float m_distance = 0f;
	Touch m_initialTouch = new Touch();

	[SerializeField] ChimpController m_chimpControlScript;

    [SerializeField] Rigidbody2D m_chimpBody2D;

    [SerializeField] float m_swipeValue;

	void Update() 
	{
		if(Time.deltaTime == 0f)
		{
			return;
		}

		foreach(Touch t in Input.touches)
		{
			if(t.phase == TouchPhase.Began)
			{
				m_initialTouch = t;
			}

			else if(t.phase == TouchPhase.Moved/* && !hasSwiped*/)
			{
				float deltaX = m_initialTouch.position.x - t.position.x;
				float deltaY = m_initialTouch.position.y - t.position.y;
				m_distance = Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
				bool swipeHorizontal = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

				if(m_distance > m_swipeValue)
				{
					if(swipeHorizontal && deltaX > 0) //Swiped Left
					{

					}

					else if(swipeHorizontal && deltaX <= 0) //Swiped Right
					{

					}

					else if(!swipeHorizontal && deltaY > 0) //Swiped Down
					{
                        Debug.Log("Swipe Down");
                        m_chimpControlScript.Slide();  
					}

					else if(!swipeHorizontal && deltaY <= 0) //Swiped Up
					{
						Debug.Log("Swipe Up");
                        m_chimpControlScript.Jump();
					}
				}
			}

			else if(t.phase == TouchPhase.Ended)
			{
				m_initialTouch = new Touch();
			}
		}
	}
}
