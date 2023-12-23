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
            // 마우스 왼쪽 버튼 입력 시
            if (PlayerControl.Instance.IsIdle())
            {
                playerRb2D.freezeRotation = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

                    if (hit.collider != null && hit.collider.tag == "Player")
                    {
                        PlayerControl.Instance.BeginGrab();
                        AudioManager.Instance.grabSound.Play();
                    }
                }
            }

            // 마우스 왼쪽 버튼 드래깅 시
            if (PlayerControl.Instance.IsGrab())
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                playerRb2D.velocity = Vector2.zero;
                playerRb2D.angularVelocity = 0f;
                player.transform.position = mousePos;

                posList.Add(mousePos);

                // 마우스 왼쪽 버튼 땔 때
                if (Input.GetMouseButtonUp(0))
                {
                    playerRb2D.freezeRotation = false;

                    throwTime = 0;
                    Vector2 minPos = findMinPos(posList);
                    Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    // 던지는 방향 벡터 계산
                    Vector2 throwVector = lastPos - minPos;
                    float throwPower = PlayerControl.Instance.GetPlayerInfo().GetThrowPower();
                    // 던진 시간이 짧을 수록 던지는 힘이 강하도록
                    // 플레이어에 힘을 가한다
                    playerRb2D.AddForce(throwVector * ((throwTime + throwPower) / throwTime), ForceMode2D.Impulse);
                    // 플레이어가 날기 시작한다.
                    PlayerControl.Instance.BeginFly();
                }
            }

            // 비행 상태 중
            if (PlayerControl.Instance.IsFly())
            {
                // 던지는 방향을 향하도록 플레이어를 회전시킨다.
                float rotateZ = CalRotateZ(new Vector2(playerRb2D.velocity.x, playerRb2D.velocity.y));
                player.transform.rotation = Quaternion.Euler(0f, 180f, -rotateZ);

                if (player.transform.position.y > maxAltitude)
                    maxAltitude = player.transform.position.y;

                // 연료 사용 시 엔진 소리 출력
                if (Input.GetKeyDown(KeyCode.Space) && fuelAmount > 0 &&
                    !PlayerControl.Instance.IsMaroPush() &&
                    releaseChichute == null)
                {
                    AudioManager.Instance.spaceEngineSound.Play();
                }

                // 연료를 다 사용하면 소리 중지
                if (fuelAmount <= 0)
                {
                    AudioManager.Instance.spaceEngineSound.Stop();
                }

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
                }

                // 연료 사용을 멈추면 소리 off
                if (Input.GetKeyUp(KeyCode.Space))
                    AudioManager.Instance.spaceEngineSound.Stop();

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

                // 고도 5,000m 이상일 때 게임 클리어
                if (player.transform.position.y >= 1000)
                {
                    PlayerControl.Instance.BeginStop();
                    SetGameEnded(true);
                }

                // 땅에 떨어지면
                if (PlayerControl.Instance.isOnGround)
                    PlayerControl.Instance.BeginLand();
            }

            // 땅에 떨어졌다면
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
                    PlayerControl.Instance.BeginStop();
                    SetGameEnded(true);
                }
            }
        }
    }

    Vector2 findMinPos(List<Vector2> posList)
    {
        Vector2 mPos = Vector2.zero;
        int lastNum = posList.Count;

        for (int i = lastNum - 1; i > 0; i--)
        {
            throwTime++;

            if (posList[i - 1].x > posList[i].x && posList[i - 1].y > posList[i].y)
            {
                mPos = posList[i];
                break;
            }
        }

        posList.Clear();
        return mPos;
    }

    private IEnumerator ReleaseChichute()
    {
        chichuteCount--;
        chichute.SetActive(true);
        float velocity_y = 0f;

        AudioManager.Instance.chicuteSound.Play();

        for (int i = 0; i < 300; i++)
        {
            velocity_y = playerRb2D.velocity.y * 0.98f;
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

    void ChangeFuelGageAmount(float amount)
    {
        fuelGage.fillAmount = amount;

        if (fuelGage.fillAmount <= 0f)
            fuelGage.fillAmount = 0f;
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
