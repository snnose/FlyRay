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

    // 부스터 사용 회수
    private int boosterCount = 0;

    // 닭하산 사용 회수
    private int chichuteCount = 0;

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
            fuelPower *= 1.2f;

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

    // 마로 관련 필드
    private bool maroTrigger = false;
    private float maroThrowForce = 1f;
    IEnumerator maroPush;

    // 트럼펫 관련 필드
    private float trumpetForce = 1f;
    private int trumpetCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 마로 강화가 됐다면 1.5배 아니면 1배
        maroThrowForce = maroThrowForce + (0.5f * DataManager.Instance.playerData.MaroUpgrade);
        // 트럼펫 강화가 됐다면 1.5배 아니면 1배
        trumpetForce = trumpetForce + (0.5f * DataManager.Instance.playerData.trumpetUpgrade);

        if (DataManager.Instance.playerData.weightUpgrade == 1)
            playerRb2D.mass *= 0.75f;

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

    // 오브젝트 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 비행 상태 혹은 착륙 상태 중일 때
        if (this.currState == PlayerInfo.state.FLIED ||
            this.currState == PlayerInfo.state.LANDED)
        {
            // 와플과 충돌하는 경우
            if (collider.gameObject.CompareTag("Waffle"))
            {
                playerInfo.GainWaffle(); // 얻은 와플 수 + 1
                AudioManager.Instance.waffleSound.Play();

                // 연료를 12.5% 충전한다
                float fuel = GameRoot.Instance.GetFuelAmount();
                if (fuel < 100f)
                {
                    GameRoot.Instance.SetFuelAmount(fuel + 12.5f);
                    GameRoot.Instance.ChangeFuelGageAmount(fuel * 0.01f + 0.125f);
                }
                Destroy(collider.gameObject);
            }
            // 마로와 충돌하는 경우
            else if (collider.gameObject.CompareTag("Maro"))
            {
                GameObject maro = collider.gameObject;
                // 닭 낙하산 사용 중에 충돌했다면
                if (GameRoot.Instance.GetChichuteCoroutine() != null)
                {
                    // 닭 낙하산 코루틴을 중지한다.
                    StopCoroutine(GameRoot.Instance.GetChichuteCoroutine());
                    GameRoot.Instance.GetChichute().SetActive(false);
                }

                // 마로 코루틴이 진행 중이 아니라면 코루틴 실행
                if (!maroTrigger)
                {
                    maroPush = MaroPush(maro);
                    StartCoroutine(maroPush);
                    maroTrigger = true;
                }
                // 마로와 충돌해 효과가 발동 중인 상태에서 다시 충돌할 때의 처리
                else
                {
                    // 현재 진행 중인 코루틴을 중지하고
                    StopCoroutine(maroPush);
                    // 새 코루틴을 넣어 다시 실행한다
                    maroPush = MaroPush(maro);
                    StartCoroutine(maroPush);
                }
            }
            // 트럼펫과 충돌하는 경우
            else if (collider.gameObject.CompareTag("Trumpet"))
            {
                Destroy(collider.gameObject);
                AudioManager.Instance.trumpetSound.Play();

                // 닭 낙하산 코루틴이 실행 중이라면
                if (GameRoot.Instance.GetChichuteCoroutine() != null)
                {
                    // 닭 낙하산 코루틴을 중지한다
                    StopCoroutine(GameRoot.Instance.GetChichuteCoroutine());
                    GameRoot.Instance.GetChichute().SetActive(false);
                }

                // 마로가 미는 상태가 아닐 때
                if (!maroTrigger)
                {
                    // 낙하하는 중에 충돌할 경우
                    if (playerRb2D.velocity.y < 0)
                        // y축 속도를 0으로 설정한다.
                        playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, 0);
                    playerRb2D.AddForce(new Vector2(20, 20) * trumpetForce, ForceMode2D.Impulse);
                }
                // 마로가 미는 중 먹었다면 그 수를 저장한다
                else
                    trumpetCount++;
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

    private IEnumerator MaroPush(GameObject maro)
    {
        yield return null;
        // 마로 애니메이션 실행
        maro.GetComponent<MaroControl>().StartPush();
        AudioManager.Instance.maroSound.Play();
        maro.transform.rotation = Quaternion.Euler(0f, 0f, -15f);

        // 충돌 직전 플레이어의 속도를 저장한다.
        Vector2 currVelocity = playerRb2D.velocity;
        currVelocity.x += 10f;
        // y축 속도를 0으로 설정하고 중력도 0으로 설정한다.
        SetVG(new Vector2(currVelocity.x, 0), 0f);
        playerRb2D.drag = 0f;   // 플레이어가 받는 공기 저항을 0으로 설정한다
        
        // 마로의 위치를 지속적으로 수정해준다
        for (int i = 0; i < 300; i++)
        {
            Vector2 player_pos = player.transform.position;
            maro.transform.position = player_pos + new Vector2(-1.15f, -0.6f);
            yield return new WaitForSeconds(0.001f);
        }
        
        // 일정 시간이 지나면 원래의 속도와 중력으로 되돌린다.
        SetVG(currVelocity, 1f);
        playerRb2D.drag = playerAirDrag;

        // 마지막으로 플레이어에 힘을 가한다.
        Vector2 Force = new Vector2(20f, 20f) * maroThrowForce + new Vector2(20f, 20f) * trumpetCount;
        playerRb2D.AddForce(Force, ForceMode2D.Impulse);
        maroTrigger = false;
        trumpetCount = 0;

        // 착륙 중에 닿았다면 다시 비행
        if (IsLand())
            currState = PlayerInfo.state.FLIED;

        Destroy(maro);
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
