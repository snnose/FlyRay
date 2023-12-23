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
        SetBoosterCount(1);
        SetChichuteCount(1);
    }

    // �÷��̾��� ���� ǥ��
    public enum state
    {
        IDLE = 0,       // ��� ��
        GRABED,         // ���콺�� ����
        FLIED,          // ���� ��
        LANDED,         // ���� ��
        STOP,           // ���� ���� ��
    };

    // ������ �� ����ġ
    private float throwPower = 0f;

    // ����
    private float fuelAmount = 0f;      // ���� ��
    private float fuelPower = 0f;           // ���� ȿ��(��)

    // �ν��� ��� ȸ��
    private int boosterCount = 0;

    // ���ϻ� ��� ȸ��
    private int chichuteCount = 0;

    // ���� ���� ��
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

    public void SetBoosterCount(int count)
    {
        if (DataManager.Instance.playerData.boosterUpgrade == 1)
            this.boosterCount = count;
        else
            this.boosterCount = 0;
    }

    public void SetChichuteCount(int count)
    {
        if (DataManager.Instance.playerData.chichuteUpgrade == 1)
            this.chichuteCount = count;
        else
            this.chichuteCount = 0;
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

    public int GetBoosterCount()
    {
        return this.boosterCount;
    }

    public int GetChichuteCount()
    {
        return this.chichuteCount;
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
    private float playerAirDrag = 0.3f;

    public PlayerInfo.state currState = PlayerInfo.state.IDLE;
    public bool isOnGround = false;
    private PlayerInfo playerInfo;

    // ���� ���� �ʵ�
    private bool maroTrigger = false;
    private float maroThrowForce = 1f;
    IEnumerator maroPush;

    // Ʈ���� ���� �ʵ�
    private float trumpetForce = 1f;
    private int trumpetCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ȭ�� �ƴٸ� 1.5�� �ƴϸ� 1��
        maroThrowForce = maroThrowForce + (0.5f * DataManager.Instance.playerData.MaroUpgrade);
        // Ʈ���� ��ȭ�� �ƴٸ� 1.5�� �ƴϸ� 1��
        trumpetForce = trumpetForce + (0.5f * DataManager.Instance.playerData.trumpetUpgrade);

        if (DataManager.Instance.playerData.weightUpgrade == 1)
            playerRb2D.mass *= 0.5f;

        playerAirDrag -= (0.09f * DataManager.Instance.playerData.airdragUpgrade);
        playerRb2D.drag = playerAirDrag;
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

    // ������Ʈ �浹 �� ó��
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.currState == PlayerInfo.state.FLIED ||
            this.currState == PlayerInfo.state.LANDED)
        {
            if (collider.gameObject.CompareTag("Waffle"))
            {
                playerInfo.GainWaffle(); // ���� ���� �� + 1
                AudioManager.Instance.waffleSound.Play();
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.CompareTag("Maro"))
            {
                GameObject maro = collider.gameObject;
                if (GameRoot.Instance.GetChichuteCoroutine() != null)
                {
                    StopCoroutine(GameRoot.Instance.GetChichuteCoroutine());
                    GameRoot.Instance.GetChichute().SetActive(false);
                }

                if (!maroTrigger)
                {
                    maroPush = MaroPush(maro);
                    StartCoroutine(maroPush);
                    maroTrigger = true;
                }
                // ���θ� �Ծ� ȿ���� �ߵ� ���� ���¿��� �� ���� ��� ó��
                else
                {
                    StopCoroutine(maroPush);
                    maroPush = MaroPush(maro);
                    StartCoroutine(maroPush);
                }
            }
            else if (collider.gameObject.CompareTag("Trumpet"))
            {
                Destroy(collider.gameObject);
                AudioManager.Instance.trumpetSound.Play();

                // ���ϻ��� ��ģ ���¶��
                if (GameRoot.Instance.GetChichuteCoroutine() != null)
                {
                    // ���ϻ� �ڷ�ƾ�� ���߰� ������ �ʰ� �Ѵ�.
                    StopCoroutine(GameRoot.Instance.GetChichuteCoroutine());
                    GameRoot.Instance.GetChichute().SetActive(false);
                }

                // ���ΰ� �̴� ���°� �ƴ� ��
                if (!maroTrigger)
                {
                    if (playerRb2D.velocity.y < 0)
                        playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, 0);
                    playerRb2D.AddForce(new Vector2(20, 20) * trumpetForce, ForceMode2D.Impulse);
                }
                // ���ΰ� �̴� �� �Ծ��ٸ� �� ���� �����Ѵ�
                else
                    trumpetCount++;
            }
            /*
            else if (collider.gameObject.CompareTag("Chicken"))
            {
                Destroy(collider.gameObject);

                if (playerRb2D.velocity.y < 0)
                    playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, 0);
                playerRb2D.AddForce(new Vector2(10, 20), ForceMode2D.Impulse);

                // ���� �߿� ������ �ٽ� ����.
                this.isOnGround = false;
                playerRb2D.freezeRotation = false;
                BeginFly();
            }
            */
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

    private IEnumerator MaroPush(GameObject maro)
    {
        yield return null;
        maro.GetComponent<MaroControl>().StartPush();
        AudioManager.Instance.maroSound.Play();
        maro.transform.rotation = Quaternion.Euler(0f, 0f, -15f);

        Vector2 currVelocity = playerRb2D.velocity;
        currVelocity.x += 10f;
        SetVG(new Vector2(currVelocity.x, 0), 0f);
        playerRb2D.drag = 0f;
        
        //yield return new WaitForSeconds(2f);
        
        for (int i = 0; i < 300; i++)
        {
            Vector2 player_pos = player.transform.position;
            maro.transform.position = player_pos + new Vector2(-1.15f, -0.6f);
            yield return new WaitForSeconds(0.001f);
        }
        
        SetVG(currVelocity, 1f);
        playerRb2D.drag = playerAirDrag;

        Vector2 Force = new Vector2(20f, 20f) * maroThrowForce + new Vector2(20f, 20f) * trumpetCount;
        playerRb2D.AddForce(Force, ForceMode2D.Impulse);
        maroTrigger = false;
        trumpetCount = 0;

        // ���� �߿� ��Ҵٸ� �ٽ� ����
        if (IsLand())
            currState = PlayerInfo.state.FLIED;

        Destroy(maro);
    }

    // �ӵ��� �߷� ����
    void SetVG(Vector2 velocity, float gravity)
    {
        if (velocity.y < 0)
            velocity.y = 0;
        playerRb2D.velocity = velocity;
        playerRb2D.gravityScale = gravity;
    }
}
