﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LandPuss : MonoBehaviour
{
    private const float DEFAULT_MOVE_SPEED = 5.0f;
    private const float DEFAULT_SLIDE_TIME = 0.5f;

    private Animator _pussAnim;
    [SerializeField] private bool _bIsGrounded , _bIsJumping , _bIsSliding , _bIsUI;
    private LandLevelManager _gameManager;
    private Rigidbody2D _pussBody2D;
	private SoundManager _soundManager;

    [SerializeField] private float _currentMoveSpeed , _currentSlideTime;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private string _holeAchievementID , _slipAchievementID , _superAchievementID;
    [SerializeField] private Transform _raycastBottom , _raycastTop;
    //[SerializeField] private Text _superTimerText; //This is for Testing only

    public bool m_isSuper;

	private void Reset()
	{
        _currentMoveSpeed = DEFAULT_MOVE_SPEED;
        _currentSlideTime = DEFAULT_SLIDE_TIME;
        _pussBody2D = GetComponent<Rigidbody2D>();
		m_isSuper = false;
        _jumpHeight = 20.5f;
        _raycastBottom = GameObject.Find("RaycastBottom").transform;
        _raycastTop = GameObject.Find("RaycastTop").transform;
        //_superTimerText = GameObject.Find("SuperTimerText").GetComponent<Text>();
	}

	private void Start()
    {
        _currentMoveSpeed = DEFAULT_MOVE_SPEED;
        _pussAnim = GetComponent<Animator>();
        _pussBody2D = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.Find("LandLevelManager").GetComponent<LandLevelManager>();
		_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        BhanuInput();
        Grounded();
        Movement();
        UICheck();
    }

    private void BhanuInput()
    {
        if(LandLevelManager.b_isUnityEditorTestingMode)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE
            if(Input.GetMouseButtonDown(0))
            {
                if((_bIsGrounded && !_bIsJumping && !_bIsSliding && !_bIsUI) || m_isSuper)
                {
                    Jump();
                }
            }

            if(Input.GetMouseButtonDown(1))
            {
                if((_bIsGrounded && !_bIsJumping && !_bIsSliding && !_bIsUI) || m_isSuper)
                {
                    Slide();
                }
            }
            #endif
        }
        
        if(SwipeManager.Instance.IsSwiping(SwipeDirection.UP))
        {
            Jump();
        }

        if(SwipeManager.Instance.IsSwiping(SwipeDirection.DOWN))
        {
            Slide();
        }
    }

    private void CheatDeath()
    {
        _gameManager.Ads();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetMoveSpeed()
    {
        return _currentMoveSpeed;
    }

    private void Grounded()
    {
        Debug.DrawLine(_raycastTop.position , _raycastBottom.position , Color.red);
        RaycastHit2D hit2D = Physics2D.Raycast(_raycastTop.position , _raycastBottom.position);

        if(hit2D)
        {
            if(hit2D.collider.gameObject.tag.Equals("Platform"))
            {
                _bIsGrounded = true;
            }

            //TODO the raycast should collide with the platform only instead of other objects so the only time the grounded becomes false is when it's not colliding with Platform instead of colliding with other object
            else if(!hit2D.collider.gameObject.tag.Equals("Platform"))
            {
                _bIsGrounded = false;
                SlideFinished();
            }
        }
        else
        {
            _bIsGrounded = false;
            SlideFinished();
        }
    }

	public void Jump()
    {
        if(_bIsGrounded && !_bIsJumping && !_bIsSliding && !_bIsUI) //This check exists in Update also for extra support as it's slow and this is for PC Game Version only
        {
            _bIsJumping = true;
            _pussAnim.SetBool("Jump" , true);
            _pussBody2D.velocity = Vector2.up * _jumpHeight; //You may experience different jump feel on different devices as Time.deltaTime not used following Code Monkey
            Invoke("JumpFinished" , 0.55f);
            //SelfieAppear();
		    _soundManager.m_soundsSource.clip = _soundManager.m_jump;

            if(SoundManager.m_playerMutedSounds == 0)
            {
                _soundManager.m_soundsSource.Play();
            }
        }
    }

    private void JumpFinished()
    {
        _pussAnim.SetBool("Jump" , false);
        _bIsJumping = false;      
        
        if(!m_isSuper)
        {
            //SelfieDisappear();
        }

        else if(m_isSuper)
        {
            //Invoke("SelfieDisappear" , 0.75f);
        }
    }

    private void Movement()
    {
        transform.Translate(Vector2.right * _currentMoveSpeed * Time.deltaTime , Space.World);
    }

    private void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Death"))
        {
			_soundManager.m_soundsSource.clip = _soundManager.m_fallDeath;

			if(SoundManager.m_playerMutedSounds == 0)
            {
                _soundManager.m_soundsSource.Play();
            }

            CheatDeath();
        }

        if(tri2D.gameObject.tag.Equals("Portal"))
		{
            //Portal.m_pickedUp++;
            //BhanuPrefs.SetPortalPickedUp(Portal.m_pickedUp);
            SceneManager.LoadScene("WaterSwimmer");
		}

		if(tri2D.gameObject.tag.Equals("Super"))
		{
            //Super.m_pickedUp++;
            //BhanuPrefs.SetSuperPickedUp(Super.m_pickedUp);
			SceneManager.LoadScene("SuperPuss");
		}
    }

    //void SelfieAppear()
    //{
    //    LandLevelManager.m_selfieButtonImage.enabled = true;
    //}

    //void SelfieDisappear()
    //{
    //    LandLevelManager.m_selfieButtonImage.enabled = false;
    //}

    private void OnTriggerStay2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Hurdle"))
        {
            if(!_bIsSliding) //TODO Since the collider triggers only once, when the puss gets back up too soon, still gets away so fixing this WIP
            {
                _soundManager.m_soundsSource.clip = _soundManager.m_hurdleDeath;

                if(SoundManager.m_playerMutedSounds == 0)
                {
                    _soundManager.m_soundsSource.Play();
                }

                CheatDeath();
            }
        }
    }

    public void Slide()
    {
		if(_bIsGrounded && !_bIsJumping && !_bIsUI)
		{
            _bIsSliding = true;
			_pussAnim.SetBool("Jog" , false);
			_pussAnim.SetBool("Slide" , true);
			Invoke("SlideFinished" , _currentSlideTime);
            //SelfieAppear();
		}
    }

    void SlideFinished()
    {
        _pussAnim.SetBool("Slide" , false);
		_bIsSliding = false;

        if(!_bIsJumping)
        {
            //SelfieDisappear();
        }
    }

    void UICheck()
    {
        if(EventSystem.current.currentSelectedGameObject != null)
        {
            _bIsUI = true;
        }

        else if(EventSystem.current.currentSelectedGameObject == null)
        {
            _bIsUI = false;
        }
    }
}
