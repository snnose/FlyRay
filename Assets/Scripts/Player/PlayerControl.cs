using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{    
    public class PlayerInfo
    {
        // 플레이어의 상태 표시
        public enum state
        {
            IDLE = 0,       // 대기 중
            GRABED,         // 마우스에 잡힘
            FLIED,          // 비행 중
            LANDED,         // 착륙 시
            STOP,           // 완전 정지 시
        };

        // 연료양
        private float fuelAmount = 100.0f;

        // 얻은 와플 수
        private int waffleCollected = 0;

        public void GainWaffle()
        {
            this.waffleCollected++;
        }

        public float GetFuelAmount()
        {
            return this.fuelAmount;
        }

        public int GetWaffleCollected()
        {
            return this.waffleCollected;
        }
    }
    private GameObject audioManager;
    public GameObject player;

    private Rigidbody2D playerRb2D;
    private AudioSource windSound;

    public PlayerInfo.state currState = PlayerInfo.state.IDLE;
    public bool isOnGround = false;
    private PlayerInfo playerInfo;

    private bool maroTrigger = false;
    IEnumerator maroPush;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");

        playerRb2D = player.GetComponent<Rigidbody2D>();
        windSound = audioManager.GetComponent<AudioSource>();

        playerInfo = new PlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
    
            
    }

    public bool IsIdle()
    {
        bool ret = false;

        if (this.currState == PlayerInfo.state.IDLE)
            ret = true;

        return ret;
    }

    public bool IsGrab()
    {
        bool ret = false;

        if (this.currState == PlayerInfo.state.GRABED)
            ret = true;

        return ret;
    }

    public bool IsFly()
    {
        bool ret = false;

        if (this.currState == PlayerInfo.state.FLIED)
            ret = true;

        return ret;
    }

    public bool IsLand()
    {
        bool ret = false;

        if (this.currState == PlayerInfo.state.LANDED)
            ret = true;

        return ret;
    }

    public bool IsStop()
    {
        bool ret = false;

        if (this.currState == PlayerInfo.state.STOP)
            ret = true;

        return ret;
    }

    public void BeginFly()
    {  
        this.currState = PlayerInfo.state.FLIED;
        windSound.volume = 0.1f;
    }

    public void BeginGrab()
    {
        this.currState = PlayerInfo.state.GRABED;
    }

    public void BeginLand()
    {
        this.currState = PlayerInfo.state.LANDED;
    }

    public void BeginStop()
    {
        this.currState = PlayerInfo.state.STOP;
        windSound.Stop();
    }

    public PlayerInfo GetPlayerInfo()
    {
        return this.playerInfo;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.currState == PlayerInfo.state.FLIED ||
            this.currState == PlayerInfo.state.LANDED)
        {
            if (collider.gameObject.CompareTag("Waffle"))
            {
                playerInfo.GainWaffle(); // 얻은 와플 수 + 1
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.CompareTag("Maro"))
            {
                Destroy(collider.gameObject);
                

                if (!maroTrigger)
                {   
                    maroPush = MaroPush();
                    StartCoroutine(maroPush);
                    maroTrigger = true;
                }
                else
                {
                    StopCoroutine(maroPush);
                    maroPush = MaroPush();
                    StartCoroutine(maroPush);
                }
                //playerRb2D.AddForce(new Vector2(10, 10), ForceMode2D.Impulse); 
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(this.currState == PlayerInfo.state.FLIED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = true;
        }
    }

    IEnumerator MaroPush()
    {
        yield return null;

        windSound.volume = 0.2f;
        Vector2 currVelocity = playerRb2D.velocity;
        currVelocity.x += 5f;
        SetVG(new Vector2(currVelocity.x, 0), 0f);
        playerRb2D.drag = 0f;
        
        yield return new WaitForSeconds(2f);
        /*
        for (int i = 0; i < 1440; i++)
        {
            //playerRb2D.AddForce(new Vector2(1, 1), ForceMode2D.Force);
            //player.transform.Translate(new Vector3(-0.05f, 0));

            yield return null;
        }
        */

        SetVG(currVelocity, 1f);
        playerRb2D.drag = 0.3f;
        windSound.volume = 0.1f;
        playerRb2D.AddForce(new Vector2 (5f, 15f), ForceMode2D.Impulse);
        maroTrigger = false;
    }

    // 속도와 중력 설정
    void SetVG(Vector2 velocity, float gravity)
    {
        playerRb2D.velocity = velocity;
        playerRb2D.gravityScale = gravity;
    }

    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(this.currState == PlayerInfo.state.LANDED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = false;
        }
    }
    */
}
