using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{    
    public class PlayerInfo
    {
        // �÷��̾��� ���� ǥ��
        public enum state
        {
            IDLE = 0,       // ��� ��
            GRABED,         // ���콺�� ����
            FLIED,          // ���� ��
            LANDED,         // ���� ��
            STOP,           // ���� ���� ��
        };

        // �����
        private float fuelAmount = 100.0f;

        // ���� ���� ��
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

    public PlayerInfo.state currState = PlayerInfo.state.IDLE;
    public bool isOnGround = false;

    public GameObject player;
    private Rigidbody2D playerRb2D;

    private PlayerInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb2D = player.GetComponent<Rigidbody2D>();

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.currState == PlayerInfo.state.FLIED)
        {
            if (collider.gameObject.CompareTag("Waffle"))
            {
                playerInfo.GainWaffle(); // ���� ���� �� + 1
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.CompareTag("Maro"))
            {
                Destroy(collider.gameObject);
                
                playerRb2D.AddForce(new Vector2(10, 10), ForceMode2D.Impulse); 
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(this.currState == PlayerInfo.state.LANDED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = false;
        }
    }
}
