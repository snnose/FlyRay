using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    private static GameRoot instance = null;

    public static GameRoot Instance
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

        Time.timeScale = 1f;

        chichute = GameObject.FindGameObjectWithTag("Chichute");
        fuelUI = GameObject.FindGameObjectWithTag("FuelUI");

        playerRb2D = player.GetComponent<Rigidbody2D>();
        fuelUIControl = fuelUI.GetComponent<FuelUIControl>();
    }

    public GameObject player;
    private GameObject chichute;
    private GameObject fuelUI;

    private FuelUIControl fuelUIControl;

    private Rigidbody2D playerRb2D;
    private Image fuelGage;

    private float fuelAmount = 0f;
    private int throwTime = 0;
    private List<Vector2> posList = new List<Vector2>();

    private float maxAltitude = 2.8f;

    private bool isGameEnded = false;   // 퍼즈 후 끝낼 때 true
    int boosterCount; int chichuteCount;

    private IEnumerator releaseChichute = null;

    // Start is called before the first frame update
    void Start()
    {
        chichute.SetActive(false);
        fuelGage = fuelUIControl.GetFuelGage();

        fuelAmount = PlayerControl.Instance.GetPlayerInfo().GetFuelAmount();
        boosterCount = PlayerControl.Instance.GetPlayerInfo().GetBoosterCount();
        chichuteCount = PlayerControl.Instance.GetPlayerInfo().GetChichuteCount();
    }

    // Update is called once per frame
    void Update()
    {
        // 퍼즈 상태가 아닐 때
        if (!PauseControl.Instance.IsPause())
        {
            // 플레이어가 대기 상태일 때
            if (PlayerControl.Instance.IsIdle())
            {
                playerRb2D.freezeRotation = true;
                // 마우스 좌클릭 입력 시
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

                    // 좌클릭 대상이 플레이어라면
                    if (hit.collider != null && hit.collider.tag == "Player")
                    {
                        // 플레이어가 잡힘 상태로 변경된다.
                        PlayerControl.Instance.BeginGrab();
                        AudioManager.Instance.grabSound.Play();
                    }
                }
            }

            // 플레이어가 잡힘 상태일 때 (좌클릭 유지 중일 때)
            if (PlayerControl.Instance.IsGrab())
            {
                // 마우스 좌표를 받아와 위치 리스트에 추가한다.
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                playerRb2D.velocity = Vector2.zero;
                playerRb2D.angularVelocity = 0f;
                player.transform.position = mousePos;

                posList.Add(mousePos);

                // 좌클릭 땔 때 (던지는 순간)
                if (Input.GetMouseButtonUp(0))
                {
                    playerRb2D.freezeRotation = false;

                    throwTime = 0;
                    Vector2 minPos = FindStartPos(posList);
                    Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    // 던지는 방향 벡터 계산
                    Vector2 throwVector = lastPos - minPos;
                    // 던지는 힘 (기본 값 50)
                    float throwPower = PlayerControl.Instance.GetPlayerInfo().GetThrowPower();
                    // 던진 시간이 짧을 수록 던지는 힘이 강하도록
                    // 플레이어에 힘을 가한다
                    playerRb2D.AddForce(throwVector * ((throwTime + throwPower) / throwTime), ForceMode2D.Impulse);
                    // 플레이어가 날기 시작한다.
                    PlayerControl.Instance.BeginFly();
                }
            }

            // 플레이어가 비행 상태일 때
            if (PlayerControl.Instance.IsFly())
            {
                // 날아가는 방향을 향하도록 플레이어를 지속적으로 회전시킨다.
                float rotateZ = CalRotateZ(new Vector2(playerRb2D.velocity.x, playerRb2D.velocity.y));
                player.transform.rotation = Quaternion.Euler(0f, 180f, -rotateZ);

                // 만약 최고 고도 기록을 갱신하면 다시 갱신한다.
                if (player.transform.position.y > maxAltitude)
                    maxAltitude = player.transform.position.y;


                // 스페이스를 누르면 연료 사용
                if (Input.GetKey(KeyCode.Space) &&
                    fuelAmount > 0 && !PlayerControl.Instance.IsMaroPush() &&
                    releaseChichute == null)
                {
                    Vector2 F = Vector2.one;
                    float fuelPower = PlayerControl.Instance.GetPlayerInfo().GetFuelPower();
                    F.y *= 2f;
                    F *= fuelPower;

                    // 힘을 가한다
                    playerRb2D.AddForce(F * 15f, ForceMode2D.Force);

                    fuelAmount -= Time.deltaTime * 100f;
                    ChangeFuelGageAmount(fuelAmount / 100);
                    // 연료 사용 소리 재생
                    AudioManager.Instance.spaceEngineSound.Play();
                }

                // 연료 사용을 멈추면 소리 off
                if (Input.GetKeyUp(KeyCode.Space) || fuelAmount <= 0)
                {
                    AudioManager.Instance.spaceEngineSound.Stop();
                }

                // 왼쪽 Control 키를 누르면 부스터
                if (Input.GetKey(KeyCode.LeftControl) &&
                    boosterCount > 0)
                {
                    playerRb2D.AddForce(new Vector2(20f, 30f), ForceMode2D.Impulse);
                    boosterCount--;
                }
                // 하강 중에 X키를 누르면 닭하산을 펼친다.
                if (Input.GetKeyDown(KeyCode.X) &&
                    playerRb2D.velocity.y < 0 &&
                    chichuteCount > 0)
                {
                    releaseChichute = ReleaseChichute();
                    StartCoroutine(releaseChichute);
                }

                // 고도 제한 시 고도 5,000m 이상일 때 게임 클리어
                if (DataManager.Instance.playerData.altitudeLimit &&
                    player.transform.position.y >= 1002.765f)
                {
                    PlayerControl.Instance.BeginStop();
                    SetGameEnded(true);
                }

                // 땅에 떨어지면
                if (PlayerControl.Instance.isOnGround)
                    // 플레이어가 착륙 상태로 변경된다.
                    PlayerControl.Instance.BeginLand();
            }

            // 플레이어가 착륙 상태라면
            if (PlayerControl.Instance.IsLand())
            {
                // 잠깐 물리 법칙을 제거하고 강제로 똑바로 서게 끔 조정한다.
                playerRb2D.isKinematic = true;
                player.transform.rotation = Quaternion.Euler(0f, 180f, 0);

                // 똑바로 섰다면 다시 물리 법칙 적용
                if (player.transform.rotation.z == 0f)
                {
                    playerRb2D.freezeRotation = true;
                    playerRb2D.isKinematic = false;
                }

                // 우주선이 멈췄다면
                if (playerRb2D.velocity == Vector2.zero)
                {
                    // 플레이어가 정지 상태로 변경된다.
                    PlayerControl.Instance.BeginStop();
                    // 게임 오버
                    SetGameEnded(true);
                }
            }
        }
    }

    // 던질 때 힘이 최대로 적용될 수 있는 시작점을 찾는다.
    Vector2 FindStartPos(List<Vector2> posList)
    {
        Vector2 mPos = Vector2.zero;
        int lastNum = posList.Count;

        // 벡터 리스트의 맨 뒤부터 탐색을 시작한다.
        // 던지는 순간 -> 처음 잡혔을 때의 위치 순서로 탐색
        for (int i = lastNum - 1; i > 0; i--)
        {
            throwTime++;
            // 현재 탐색 중인 x, y 위치 값이 이전 위치 값보다 작다면
            if (posList[i - 1].x > posList[i].x && posList[i - 1].y > posList[i].y)
            {
                // 힘이 작용되는 시작점을 정한다.
                mPos = posList[i];
                break;
            }
        }

        posList.Clear();
        return mPos;
    }

    // 닭 낙하산을 펼치는 코루틴 함수
    private IEnumerator ReleaseChichute()
    {
        // 닭 낙하산 사용 횟수를 차감한다.
        chichuteCount--;
        // 닭 낙하산 오브젝트를 보이게 변경
        chichute.SetActive(true);
        float velocity_y = 0f;  // y축 속도

        AudioManager.Instance.chicuteSound.Play();

        // 400프레임 동안 지속한다.
        for (int i = 0; i < 400; i++)
        {
            // 현재 플레이어의 y축 속도를 점점 감소시킨다.
            velocity_y = playerRb2D.velocity.y * 0.98f;
            // x축 방향으로 힘을 조금씩 가해 가속을 준다.
            playerRb2D.AddForce(new Vector2(4.0f, 0f));
            playerRb2D.velocity = new Vector2 (playerRb2D.velocity.x, velocity_y);
            yield return new WaitForSeconds(0.001f);
        }

        yield return null;
        chichute.SetActive(false);
        yield return null;
        releaseChichute = null;
    }

    public IEnumerator GetChichuteCoroutine()
    {
        return this.releaseChichute;
    }

    private float CalRotateZ(Vector2 v)
    {
        float f = 0f;
        Vector2 normV = v.normalized;

        f = Mathf.Atan2(normV.y ,normV.x) * Mathf.Rad2Deg;

        return f;
    }

    public void SetFuelAmount(float amount)
        {
            if (amount >= 100f)
                amount = 100f;

            this.fuelAmount = amount;
        }

    public float GetFuelAmount()
    {
        return this.fuelAmount;
    }

    public void ChangeFuelGageAmount(float amount)
    {
        fuelGage.fillAmount = amount;

        if (fuelGage.fillAmount <= 0f)
            fuelGage.fillAmount = 0f;

        if (fuelGage.fillAmount >= 1f)
            fuelGage.fillAmount = 1f;
    }
    
    public void SetGameEnded(bool end)
    {
        this.isGameEnded = end;
    }

    public bool IsGameEnded()
    {
        return this.isGameEnded;
    }

    public float GetMaxAltitude()
    {
        return this.maxAltitude;
    }

    public GameObject GetChichute()
    {
        return this.chichute;
    }
}
