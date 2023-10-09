using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{    
    public class Player
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

    }

    public Player.state currState = Player.state.IDLE;
    public bool isOnGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isIdle()
    {
        bool ret = false;

        if (this.currState == Player.state.IDLE)
            ret = true;

        return ret;
    }

    public bool isGrab()
    {
        bool ret = false;

        if (this.currState == Player.state.GRABED)
            ret = true;

        return ret;
    }

    public bool isFly()
    {
        bool ret = false;

        if (this.currState == Player.state.FLIED)
            ret = true;

        return ret;
    }

    public bool isLand()
    {
        bool ret = false;

        if (this.currState == Player.state.LANDED)
            ret = true;

        return ret;
    }

    public void beginFly()
    {
        this.currState = Player.state.FLIED;
    }

    public void beginGrab()
    {
        this.currState = Player.state.GRABED;
    }

    public void beginLand()
    {
        this.currState = Player.state.LANDED;
    }

    public void beginStop()
    {
        this.currState = Player.state.STOP;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.currState == Player.state.FLIED &&
            collider.gameObject.CompareTag("Waffle"))
        {
            //Debug.Log("���� �ȳ�");
            Destroy(collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(this.currState == Player.state.FLIED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(this.currState == Player.state.LANDED &&
            collision.collider.gameObject.CompareTag("Ground"))
        {
            this.isOnGround = false;
        }
    }
}
