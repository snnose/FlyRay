using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInfo()
    {
        SetThrowPower(50f);
        SetFuelPower(1f);
        SetFuelAmount(100f);
    }

    // 플레이어의 상태 표시
    public enum state
    {
        IDLE = 0,       // 대기 중
        GRABED,         // 마우스에 잡힘
        FLIED,          // 비행 중
        LANDED,         // 착륙 시
        STOP,           // 완전 정지 시
    };

    // 던지는 힘 보정치
    private float throwPower = 0f;

    // 연료
    private float fuelAmount = 0f;      // 연료 양
    private float fuelPower = 0f;           // 연료 효율(힘)

    // 얻은 와플 수
    private int waffleCollected = 0;

    public void GainWaffle()
    {
        this.waffleCollected++;
    }

    public void SetThrowPower(float throwPower)
    {
        if (DataManager.Instance.playerData.throwUpgrade == 1)
            throwPower *= 1.5f;

        this.throwPower = throwPower;
    }

    public void SetFuelPower(float fuelPower)
    {
        if (DataManager.Instance.playerData.fuelUpgrade == 1)
            fuelPower *= 1.5f;

        this.fuelPower = fuelPower;
    }

    public void SetFuelAmount(float fuelAmount)
    {


        this.fuelAmount = fuelAmount;
    }

    public float GetThrowPower()
    {
        return this.throwPower;
    }

    public float GetFuelPower()
    {
        return this.fuelPower;
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

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance = null;
    
    public static PlayerControl Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");

        playerRb2D = player.GetComponent<Rigidbody2D>();

        playerInfo = new PlayerInfo();
    }

    private GameObject player;
    
    private Rigidbody2D playerRb2D;

    public PlayerInfo.state currState = PlayerInfo.state.IDLE;
    public bool isOnGround = false;
    private PlayerInfo playerInfo;

    private bool maroTrigger = false;
    IEnumerator maroPush;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public bool IsMaroPush()
    {
        bool ret = false;

        if (this.maroTrigger)
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
    }

    public PlayerInfo GetPlayerInfo()
    {
        return this.playerInfo;
    }

    public GameObject GetPlayer()
    {
        return this.player;
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
            }
            else if (collider.gameObject.CompareTag("Chicken"))
            {
                Destroy(collider.gameObject);

                if (playerRb2D.velocity.y < 0)
                    playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, 0);
                playerRb2D.AddForce(new Vector2(5, 20), ForceMode2D.Impulse);

                // 착륙 중에 먹으면 다시 난다.
                this.isOnGround = false;
                playerRb2D.freezeRotation = false;
                BeginFly();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.currState == PlayerInfo.state.FLIED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = true;
        }
    }

    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (this.currState == PlayerInfo.state.LANDED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = false;
            BeginFly();
        }
    }
    */

    IEnumerator MaroPush()
    {
        yield return null;

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
        playerRb2D.AddForce(new Vector2 (5f, 15f), ForceMode2D.Impulse);
        maroTrigger = false;

        // 착륙 중에 닿았다면 다시 비행
        if (IsLand())
            currState = PlayerInfo.state.FLIED;
    }

    // 속도와 중력 설정
    void SetVG(Vector2 velocity, float gravity)
    {
        if (velocity.y < 0)
            velocity.y = 0;
        playerRb2D.velocity = velocity;
        playerRb2D.gravityScale = gravity;
    }
}
