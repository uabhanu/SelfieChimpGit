﻿using UnityEngine;


public class LevelCreator : MonoBehaviour
{
    bool m_collectibleAdded , m_enemyAdded;
    LandChimp m_landChimp;
    const float m_tileWidth = 1.25f;
    float m_blankCounter = 0 , m_outofbounceX , m_startUpPosY;
    GameObject m_collectedTiles , m_gameLayer ,  m_tmpTile;
	int m_heightLevel = 0;
	string m_lastTile = "PF_GroundRight";

    public float m_gameSpeed;
    public GameObject m_tilePos;
    public static float m_middleCounter = 0;

	void Start() 
	{
        m_collectedTiles = GameObject.Find("Tiles");
        m_gameLayer = GameObject.Find("GameLayer");
        m_landChimp = GameObject.Find("LandChimp").GetComponent<LandChimp>();

		for(int i = 0; i < 30; i++)
        {
			GameObject tmpG1 = Instantiate(Resources.Load("PF_GroundLeft" , typeof(GameObject))) as GameObject;
			tmpG1.transform.parent = m_collectedTiles.transform.Find("gLeft").transform;
			tmpG1.transform.position = Vector2.zero;

			GameObject tmpG2 = Instantiate(Resources.Load("PF_GroundMiddle" , typeof(GameObject))) as GameObject;
			tmpG2.transform.parent = m_collectedTiles.transform.Find("gMiddle").transform;
			tmpG2.transform.position = Vector2.zero;

			GameObject tmpG3 = Instantiate(Resources.Load("PF_GroundRight" , typeof(GameObject))) as GameObject;
			tmpG3.transform.parent = m_collectedTiles.transform.Find("gRight").transform;
			tmpG3.transform.position = Vector2.zero;

			GameObject tmpG4 = Instantiate(Resources.Load("PF_Blank" , typeof(GameObject))) as GameObject;
			tmpG4.transform.parent = m_collectedTiles.transform.Find("gBlank").transform;
			tmpG4.transform.position = Vector2.zero;
		}

        for(int i = 0; i < 10; i++)
        {
            GameObject hurdle = Instantiate(Resources.Load("PF_Hurdle" , typeof(GameObject))) as GameObject;
            hurdle.transform.parent = m_collectedTiles.transform.Find("Hurdle").transform;
            hurdle.transform.position = Vector2.zero;
        }

        for(int i = 0; i < 5; i++)
        {
            GameObject bananaSkin = Instantiate(Resources.Load("PF_BananaSkin", typeof(GameObject))) as GameObject;
            bananaSkin.transform.parent = m_collectedTiles.transform.Find("Skin").transform;
            bananaSkin.transform.position = Vector2.zero;
        }

        m_collectedTiles.transform.position = new Vector2 (-60.0f , -20.0f);

		m_tilePos = GameObject.Find("StartTilePosition");
		m_startUpPosY = m_tilePos.transform.position.y;
		m_outofbounceX = m_tilePos.transform.position.x - 5.0f;
		FillScene();

        Invoke("GameSpeed" , 5.8f);
    }

	void FixedUpdate()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        LevelGeneration();
    }

    void ChangeHeight()
    {
        int newHeightLevel = Random.Range(0 , 2);

        if(newHeightLevel < m_heightLevel)
        {
            m_heightLevel--;
        }

        else if(newHeightLevel > m_heightLevel)
        {
            m_heightLevel++;
        }
    }

    void FillScene()
	{
		for(int i = 0; i < 15; i++)
		{
			SetTile("PF_GroundMiddle");
		}

		SetTile("PF_GroundRight");
	}

    void GameSpeed()
    {
        if(m_gameSpeed < 12f && !m_landChimp.m_isSuper)
        {
            m_gameSpeed += 0.5f;
        }

        if(m_gameSpeed > 12f)
        {
            m_gameSpeed = 12f;
        }
        
        Invoke("GameSpeed" , 5.8f);
    }

    void LevelGeneration()
    {
        
        m_gameLayer.transform.position = new Vector2(m_gameLayer.transform.position.x - m_gameSpeed * Time.deltaTime , 0);

        for(int i = 0; i < m_gameLayer.transform.childCount; i++)
        {
            Transform child  = m_gameLayer.transform.GetChild(i);

            if(child.position.x < m_outofbounceX)
            {
                switch(child.gameObject.name)
                {
                    case "PF_BananaSkin(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("Skin").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("Skin").transform;
                    break;

                    case "PF_Blank(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("gBlank").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("gBlank").transform;
                    break;

                    case "PF_GroundLeft(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("gLeft").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("gLeft").transform;
                    break;

                    case "PF_GroundMiddle(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("gMiddle").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("gMiddle").transform;
                    break;

                    case "PF_GroundRight(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("gRight").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("gRight").transform;
                    break;

                    case "PF_Hurdle(Clone)":
                        child.gameObject.transform.position = m_collectedTiles.transform.Find("Hurdle").transform.position;
                        child.gameObject.transform.parent = m_collectedTiles.transform.Find("Hurdle").transform;
                    break;

                    default:
                        Destroy(child.gameObject);
                    break;
                }
            }
        }

        if(m_gameLayer.transform.childCount < 25)
        {
            SpawnTile();
        }
    }

    void RandomizeEnemy()
    {
        if(m_enemyAdded)
        {
            return;
        }

        if(Random.Range(0 , 10) == 1)
        {

            GameObject hurdle = m_collectedTiles.transform.Find("Hurdle").transform.GetChild(0).gameObject;
            hurdle.transform.parent = m_gameLayer.transform;
            hurdle.transform.position = new Vector2(m_tilePos.transform.position.x + m_tileWidth * 3 , m_startUpPosY + (m_heightLevel * m_tileWidth + (m_tileWidth * 2.3f)));
            m_enemyAdded = true;
        }

        else if(Random.Range(0 , 10) == 1)
        {
            GameObject skin = m_collectedTiles.transform.Find("Skin").transform.GetChild(0).gameObject;
            skin.transform.parent = m_gameLayer.transform;
            skin.transform.position = new Vector2(m_tilePos.transform.position.x + m_tileWidth * 3.7f , m_startUpPosY + (m_heightLevel * m_tileWidth + (m_tileWidth * 2f)));
            m_enemyAdded = true;
        }
    }

    void SetTile(string type)
	{
		switch(type)
        {
            case "PF_Blank":
			    m_tmpTile = m_collectedTiles.transform.Find("gBlank").transform.GetChild(0).gameObject;
		    break;

		    case "PF_GroundLeft":
			    m_tmpTile = m_collectedTiles.transform.Find("gLeft").transform.GetChild(0).gameObject;
		    break;

		    case "PF_GroundMiddle":
			    m_tmpTile = m_collectedTiles.transform.Find("gMiddle").transform.GetChild(0).gameObject;
		    break;

		    case "PF_GroundRight":
			    m_tmpTile = m_collectedTiles.transform.Find("gRight").transform.GetChild(0).gameObject;
		    break;
        }

		m_tmpTile.transform.parent = m_gameLayer.transform;
		m_tmpTile.transform.position = new Vector3(m_tilePos.transform.position.x + (m_tileWidth) , m_startUpPosY + (m_heightLevel * m_tileWidth) , 0);
		m_tilePos = m_tmpTile;
		m_lastTile = type;
	}

	void SpawnTile()
    {
		if(m_blankCounter > 0)
        {
			SetTile("PF_Blank");
			m_blankCounter--;
            return;
		}

        if(m_middleCounter > 0)
        {
            SetTile("PF_GroundMiddle");
            m_middleCounter--;
			return;
		}

        m_enemyAdded = false;

		if(m_lastTile == "PF_Blank")
        {
			ChangeHeight();
			SetTile("PF_GroundLeft");
            m_middleCounter = Random.Range(1 , 8);
            

            if(m_middleCounter > 4)
            {
                RandomizeEnemy();
            }
		}
        
        else if(m_lastTile == "PF_GroundRight")
        {
			m_blankCounter = Random.Range(1 , 3);
		}
        
        else if(m_lastTile == "PF_GroundMiddle")
        {
			SetTile("PF_GroundRight");
		}
	}
}
