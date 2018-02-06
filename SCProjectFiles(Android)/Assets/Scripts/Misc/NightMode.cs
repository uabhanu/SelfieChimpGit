﻿using System;
using System.Collections;
using UnityEngine;

public class NightMode : MonoBehaviour
{
    bool m_nightMode = false;
    DateTime m_dateAndTime;
    SpriteRenderer m_spriteRenderer;

    [SerializeField] int m_hour;
    [SerializeField] Sprite m_nightSprite;

    void Reset()
    {
        m_hour = 18;
    }

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_dateAndTime = DateTime.Now;

        if(m_dateAndTime.Hour >= m_hour)
        {
            m_spriteRenderer.sprite = m_nightSprite;
            m_nightMode = true;   
        }

        StartCoroutine("NightRoutine");
    }

    IEnumerator NightRoutine()
    {
        //Debug.Log("Night Mode");

        m_dateAndTime = DateTime.Now;

        yield return new WaitForSeconds(2.2f);
        
        if(m_dateAndTime.Hour >= m_hour)
        {
            m_spriteRenderer.sprite = m_nightSprite;
            m_nightMode = true;   
        }

        StartCoroutine("NightRoutine");
    }
}