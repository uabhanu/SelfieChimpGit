﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChimpController : MonoBehaviour 
{
    //private BoxCollider2D superchimpCollider2D;
    //private GameObject superChimp;
    BananaSkin m_bananaSkinScript;
    BoxCollider2D m_chimpCollider2D;
    Rigidbody2D m_chimpBody2D;

	[SerializeField] GameObject /*m_dollarButtonObj ,*/ m_pauseButtonObj , m_selfieButtonObj;

	public Animator m_chimpAnim;
	public AudioSource m_deathSound;
	public AudioSource m_jumpSound;
	public bool m_canJump , m_chimpInTheHole , m_clickEnableTest;
	public bool m_chimpSlip;
	public bool m_grounded;
	public bool m_slide;
	public bool m_superMode;
	public float m_chimpSpeed;
	public float m_groundCheckRadius;
	public float m_jumpHeight;
	public float m_slideTime;
	public float m_slipTime;
	public float m_superTime;
	public GameObject m_chimpBlocker;
	public GameManager m_gameManagementScript;
	public Ground m_groundScript;
	public LayerMask m_whatIsGround;
	public ScoreManager m_scoreManagementScript;
	public SpriteRenderer m_superChimpRenderer;
	public Transform m_groundCheck;

    void Reset()
    {
        m_chimpSpeed = 0f;
        m_groundCheckRadius = 0.8f;
        m_jumpHeight = 17f;
        m_slideTime = 0.75f;
        m_slipTime = 10f;
        m_superTime = 15f;
    }

    void Start() 
	{
        m_canJump = true;
		m_chimpAnim.SetBool("DefaultSpeed" , true);
		m_chimpAnim.SetBool("MediumSpeed" , false);
		m_chimpAnim.SetBool("HighSpeed" , false);
		m_chimpBody2D = GetComponent<Rigidbody2D>();
        m_chimpCollider2D = GetComponent<BoxCollider2D>();
        m_chimpInTheHole = false;
        StartCoroutine("GetScriptsRoutine");
	}

	void FixedUpdate()
	{
		if(Time.timeScale == 0f)
		{
			return;
		}
			
		if(!m_superMode) 
		{			
			m_chimpAnim.SetBool("Super" , false);
			m_grounded = Physics2D.OverlapCircle(m_groundCheck.position , m_groundCheckRadius , m_whatIsGround);

            if(m_grounded)
            {
                m_canJump = true;
                m_clickEnableTest = false;

                if(!m_chimpAnim.GetBool("Slide"))
                {
                    m_selfieButtonObj.SetActive(false);
                }

                if(m_chimpAnim.GetBool("Slide"))
                {
                    m_selfieButtonObj.SetActive(true);
                }
            }

            if(!m_grounded)
            {
                m_canJump = false;
                m_clickEnableTest = true;
                m_selfieButtonObj.SetActive(true);
            }
		} 

		else if(m_superMode) 
		{
			m_chimpAnim.SetBool("Super" , true);
		}

		m_slide = m_chimpAnim.GetBool("Slide");
		Run();
	}

	void Update() 
	{
		if(Time.timeScale == 0f)
		{
			return;
		}

#if UNITY_EDITOR_64

        if(Input.GetMouseButton(0))
		{
			Jump();
		}

		else if(Input.GetMouseButton(1))
		{
			Slide();
		}

		#endif
	}

    IEnumerator ChimpCollider2D()
    {
        yield return new WaitForSeconds(0.2f);
        m_chimpCollider2D.isTrigger = false;
    }

	IEnumerator ChimpSlip()
	{
		yield return new WaitForSeconds(m_slipTime);
		m_chimpSlip = false;
		m_chimpAnim.SetBool("DefaultSpeed" , true);
		m_chimpAnim.SetBool("MediumSpeed" , false);
        m_chimpAnim.SetBool("HighSpeed" , false);
		m_groundScript.speed = 4f;
	}

    IEnumerator GetScriptsRoutine()
    {
        yield return new WaitForSeconds(1f);
        m_bananaSkinScript = FindObjectOfType<BananaSkin>();
        StartCoroutine("GetScriptsRoutine");
    }

	IEnumerator SlideTimer()
	{
		yield return new WaitForSeconds(m_slideTime);

		if(m_chimpAnim.GetBool("Slide"))
		{
            //Debug.Log("Slide Time"); Working
            m_canJump = true;
			m_chimpAnim.SetBool("Slide" , false);
		}
	}

	IEnumerator SuperChimpTimer()
	{
		yield return new WaitForSeconds(m_superTime);
		m_chimpBlocker.SetActive(false);
		m_chimpBody2D.gravityScale = 5f;
		m_superMode = false;
	}
		
	void Death()
	{
		//Debug.Log("Player Died"); Working
		m_deathSound.Play();
		m_gameManagementScript.RestartGame();
        m_selfieButtonObj.SetActive(false);
		m_pauseButtonObj.SetActive(false);
	}

	void Dying()
	{

	}

	public void Jump()
	{
        if(!m_chimpInTheHole && m_canJump && m_clickEnableTest)
        {
            m_jumpSound.Play(); //Turned off for testing purposes but turn back on for final version
            m_chimpBody2D.velocity = new Vector2(m_chimpBody2D.velocity.x , m_jumpHeight);
            m_chimpBlocker.SetActive(false);

            if(m_superMode)
            {
                m_chimpBody2D.velocity = new Vector2(m_chimpBody2D.velocity.x, m_jumpHeight * 1.1f);
            }
        }
	}

	void OnTriggerEnter2D(Collider2D tri2D)
	{
        if(tri2D.gameObject.name.Equals("PF_BananaSkin(Clone)"))
        {
            if(!m_superMode)
            {
                //Debug.Log("Chimp Slip"); //Working
                m_chimpAnim.SetBool("DefaultSpeed", false);
                m_chimpAnim.SetBool("MediumSpeed", true);
                m_chimpSlip = true;
                m_groundScript.speed += 4f;
                StartCoroutine("ChimpSlip");
            }
        }

        if(tri2D.gameObject.tag.Equals("Death"))
        {
            Debug.Log("Chimp Died");
            Death();
        }

        if(tri2D.gameObject.tag.Equals("SC"))
		{
			SuperChimp();
		}
	}

    void Run()
    {
        m_chimpAnim.SetBool("Grounded" , m_grounded);
    }

    public void Slide()
	{
        m_canJump = false;

        if(!m_chimpInTheHole)
        {
            //Debug.Log("Slide"); //Working
            m_chimpAnim.SetBool("Slide", true);
            StartCoroutine("SlideTimer");
        }
	}

	void SuperChimp()
	{
        m_canJump = true;
        m_chimpBlocker.SetActive(true);
		m_chimpBody2D.gravityScale = 3.5f;

		if(m_groundScript.speed == 8f)
		{
			m_groundScript.speed = 4f;
		}
			
		m_superMode = true;
		StartCoroutine("SuperChimpTimer");
	}
}
