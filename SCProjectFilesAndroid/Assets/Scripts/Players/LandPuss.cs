﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandPuss : MonoBehaviour
{
    private Animator _pussAnim;
    private bool _bIsGrounded , _bHighSlip , _bIsJumping , _bLowSlip , _bIsSliding , _bIsUI;
    private float _currentMoveSpeed;
    private GameManager _gameManager;
    private GameObject[] _rockObjs;
    private Rigidbody2D _pussBody2D;
	private SoundManager _soundManager;

    [SerializeField] private float _defaultMoveSpeed , _defaultGravityScale , _jumpHeight , _slipTime , _superTime;
    [SerializeField] private GameObject _bottomWall , _topWall;
    [SerializeField] private PlatformsGenerator _platformsGenerator;
    [SerializeField] private RocksGenerator _rocksGenerator;
    [SerializeField] private string _holeAchievementID , _slipAchievementID , _superAchievementID;
    [SerializeField] private Transform _raycastBottom , _raycastTop;
    [SerializeField] private Text _superTimerText; //This is for Testing only

    public bool m_isSlipping , m_isSuper;

	void Reset()
	{
        _defaultMoveSpeed = 5.0f;
        m_isSlipping = false;
		m_isSuper = false;
        _jumpHeight = 20.5f;
        _raycastBottom = GameObject.Find("RaycastBottom").transform;
        _raycastTop = GameObject.Find("RaycastTop").transform;
        _slipTime = 30.25f;
        _superTime = 30.25f;
	}

	void Start()
    {
        _currentMoveSpeed = _defaultMoveSpeed;
        _pussAnim = GetComponent<Animator>();
        _pussBody2D = GetComponent<Rigidbody2D>();
        _defaultGravityScale = _pussBody2D.gravityScale;
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
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

    void BhanuInput()
    {
        if(GameManager.b_isUnityEditorTestingMode)
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

    void CheatDeath()
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

    void Grounded()
    {
        if(!m_isSuper)
        {
            //Debug.DrawLine(_raycastTop.position , _raycastBottom.position , Color.red);
            RaycastHit2D hit2D = Physics2D.Raycast(_raycastTop.position , _raycastBottom.position);

            if(hit2D)
            {
                if(hit2D.collider.gameObject.tag.Equals("Platform"))
                {
                    _bIsGrounded = true;
                }

                else if(!hit2D.collider.gameObject.tag.Equals("Platform"))
                {
                    _bIsGrounded = false;
                    SlideFinished();
                }

                else
                {
                    _bIsGrounded = false;
                    SlideFinished();
                }
            }

            else if(!hit2D)
            {
                _bIsGrounded = false;
                SlideFinished();
            }

            else
            {
                _bIsGrounded = false;
                SlideFinished();
            }
        }

        //TODO The block below is for testing only so remove after Super Chimp Logic successful
        if(m_isSuper)
        {
            _superTime -= Time.deltaTime;
            _superTimerText.text = _superTime.ToString();
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

            if(_soundManager.m_soundsSource.enabled)
            {
                _soundManager.m_soundsSource.Play();
            }
        }
        
        if(m_isSuper)
        {
            _pussBody2D.velocity = new Vector2(_pussBody2D.velocity.x , _jumpHeight);
        }
    }

    void JumpFinished()
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

    void Movement()
    {
        transform.Translate(-Vector2.left * _currentMoveSpeed * Time.deltaTime , Space.Self);
    }

    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Killbox"))
        {
			_soundManager.m_soundsSource.clip = _soundManager.m_fallDeath;

			if(_soundManager.m_soundsSource.enabled)
            {
                _soundManager.m_soundsSource.Play();
            }

            CheatDeath();
        }

        if(tri2D.gameObject.tag.Equals("Hurdle"))
        {
			_soundManager.m_soundsSource.clip = _soundManager.m_hurdleDeath;

			if(_soundManager.m_soundsSource.enabled)
            {
                _soundManager.m_soundsSource.Play();
            }

            CheatDeath();
        }

		if(tri2D.gameObject.tag.Equals("Portal"))
		{
            SceneManager.LoadScene("WaterSwimmer");
		}

        if(tri2D.gameObject.tag.Equals("Skin"))
        {
            Slip();
        }

		if(tri2D.gameObject.tag.Equals("Super"))
		{
			Super();
		}
    }

    //void SelfieAppear()
    //{
    //    GameManager.m_selfieButtonImage.enabled = true;
    //}

    //void SelfieDisappear()
    //{
    //    GameManager.m_selfieButtonImage.enabled = false;
    //}

    public void Slide()
    {
		if(_bIsGrounded && !_bIsJumping && !_bIsUI)
		{
            _bIsSliding = true;
			_pussAnim.SetBool("Jog" , false);
			_pussAnim.SetBool("Slide" , true);
			Invoke("SlideFinished" , 0.75f);
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

    void Slip()
    {
        _pussAnim.SetBool("Slip" , true);
        Invoke("SlipFinished" , _slipTime);
    }

    void SlipFinished()
    {
		_pussAnim.SetBool("Slip" , false);

        if(!m_isSuper)
        {
            if(_bHighSlip)
            {
                _bHighSlip = false;
                _bLowSlip = false;
            }
            
            else if(_bLowSlip)
            {
                _bHighSlip = false;
                _bLowSlip = false;
            }
        }
        
        m_isSlipping = false;
    }

	void Super()
	{
        _bottomWall.SetActive(true);
        _bIsGrounded = false;
        _currentMoveSpeed *= 2;
        m_isSuper = true;
        _pussAnim.SetBool("Super" , true);
        _pussBody2D.gravityScale = 3;
		//SelfieAppear();
        SlipFinished();
        _topWall.SetActive(true);
		Invoke("SuperFinished" , _superTime);
	}

    void SuperFinished()
    {
        _rockObjs = GameObject.FindGameObjectsWithTag("Rock");

        for(int i = 0 ; i < _rockObjs.Length ; i++)
        {
            if(_rockObjs[i].gameObject.transform.position.y > -4.50f)
            {
                Destroy(_rockObjs[i].gameObject);
            }
        }

        _bottomWall.SetActive(false);
        _currentMoveSpeed = _defaultMoveSpeed;
        _pussBody2D.gravityScale = _defaultGravityScale;
        _pussAnim.SetBool("Super" , false);
        m_isSuper = false;	
        _topWall.SetActive(false);
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
