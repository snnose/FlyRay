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

    private bool isGameEnded = false;   // ���� �� ���� �� true
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
        // ���� ���°� �ƴ� ��
        if (!PauseControl.Instance.IsPause())
        {
            // �÷��̾ ��� ������ ��
            if (PlayerControl.Instance.IsIdle())
            {
                playerRb2D.freezeRotation = true;
                // ���콺 ��Ŭ�� �Է� ��
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f);

                    // ��Ŭ�� ����� �÷��̾���
                    if (hit.collider != null && hit.collider.tag == "Player")
                    {
                        // �÷��̾ ���� ���·� ����ȴ�.
                        PlayerControl.Instance.BeginGrab();
                        AudioManager.Instance.grabSound.Play();
                    }
                }
            }

            // �÷��̾ ���� ������ �� (��Ŭ�� ���� ���� ��)
            if (PlayerControl.Instance.IsGrab())
            {
                // ���콺 ��ǥ�� �޾ƿ� ��ġ ����Ʈ�� �߰��Ѵ�.
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                playerRb2D.velocity = Vector2.zero;
                playerRb2D.angularVelocity = 0f;
                player.transform.position = mousePos;

                posList.Add(mousePos);

                // ��Ŭ�� �� �� (������ ����)
                if (Input.GetMouseButtonUp(0))
                {
                    playerRb2D.freezeRotation = false;

                    throwTime = 0;
                    Vector2 minPos = FindStartPos(posList);
                    Vector2 lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    // ������ ���� ���� ���
                    Vector2 throwVector = lastPos - minPos;
                    // ������ �� (�⺻ �� 50)
                    float throwPower = PlayerControl.Instance.GetPlayerInfo().GetThrowPower();
                    // ���� �ð��� ª�� ���� ������ ���� ���ϵ���
                    // �÷��̾ ���� ���Ѵ�
                    playerRb2D.AddForce(throwVector * ((throwTime + throwPower) / throwTime), ForceMode2D.Impulse);
                    // �÷��̾ ���� �����Ѵ�.
                    PlayerControl.Instance.BeginFly();
                }
            }

            // �÷��̾ ���� ������ ��
            if (PlayerControl.Instance.IsFly())
            {
                // ���ư��� ������ ���ϵ��� �÷��̾ ���������� ȸ����Ų��.
                float rotateZ = CalRotateZ(new Vector2(playerRb2D.velocity.x, playerRb2D.velocity.y));
                player.transform.rotation = Quaternion.Euler(0f, 180f, -rotateZ);

                // ���� �ְ� �� ����� �����ϸ� �ٽ� �����Ѵ�.
                if (player.transform.position.y > maxAltitude)
                    maxAltitude = player.transform.position.y;


                // �����̽��� ������ ���� ���
                if (Input.GetKey(KeyCode.Space) &&
                    fuelAmount > 0 && !PlayerControl.Instance.IsMaroPush() &&
                    releaseChichute == null)
                {
                    Vector2 F = Vector2.one;
                    float fuelPower = PlayerControl.Instance.GetPlayerInfo().GetFuelPower();
                    F.y *= 2f;
                    F *= fuelPower;

                    // ���� ���Ѵ�
                    playerRb2D.AddForce(F * 15f, ForceMode2D.Force);

                    fuelAmount -= Time.deltaTime * 100f;
                    ChangeFuelGageAmount(fuelAmount / 100);
                    // ���� ��� �Ҹ� ���
                    AudioManager.Instance.spaceEngineSound.Play();
                }

                // ���� ����� ���߸� �Ҹ� off
                if (Input.GetKeyUp(KeyCode.Space) || fuelAmount <= 0)
                {
                    AudioManager.Instance.spaceEngineSound.Stop();
                }

                // ���� Control Ű�� ������ �ν���
                if (Input.GetKey(KeyCode.LeftControl) &&
                    boosterCount > 0)
                {
                    playerRb2D.AddForce(new Vector2(20f, 30f), ForceMode2D.Impulse);
                    boosterCount--;
                }
                // �ϰ� �߿� XŰ�� ������ ���ϻ��� ��ģ��.
                if (Input.GetKeyDown(KeyCode.X) &&
                    playerRb2D.velocity.y < 0 &&
                    chichuteCount > 0)
                {
                    releaseChichute = ReleaseChichute();
                    StartCoroutine(releaseChichute);
                }

                // �� ���� �� �� 5,000m �̻��� �� ���� Ŭ����
                if (DataManager.Instance.playerData.altitudeLimit &&
                    player.transform.position.y >= 1002.765f)
                {
                    PlayerControl.Instance.BeginStop();
                    SetGameEnded(true);
                }

                // ���� ��������
                if (PlayerControl.Instance.isOnGround)
                    // �÷��̾ ���� ���·� ����ȴ�.
                    PlayerControl.Instance.BeginLand();
            }

            // �÷��̾ ���� ���¶��
            if (PlayerControl.Instance.IsLand())
            {
                // ��� ���� ��Ģ�� �����ϰ� ������ �ȹٷ� ���� �� �����Ѵ�.
                playerRb2D.isKinematic = true;
                player.transform.rotation = Quaternion.Euler(0f, 180f, 0);

                // �ȹٷ� ���ٸ� �ٽ� ���� ��Ģ ����
                if (player.transform.rotation.z == 0f)
                {
                    playerRb2D.freezeRotation = true;
                    playerRb2D.isKinematic = false;
                }

                // ���ּ��� ����ٸ�
                if (playerRb2D.velocity == Vector2.zero)
                {
                    // �÷��̾ ���� ���·� ����ȴ�.
                    PlayerControl.Instance.BeginStop();
                    // ���� ����
                    SetGameEnded(true);
                }
            }
        }
    }

    // ���� �� ���� �ִ�� ����� �� �ִ� �������� ã�´�.
    Vector2 FindStartPos(List<Vector2> posList)
    {
        Vector2 mPos = Vector2.zero;
        int lastNum = posList.Count;

        // ���� ����Ʈ�� �� �ں��� Ž���� �����Ѵ�.
        // ������ ���� -> ó�� ������ ���� ��ġ ������ Ž��
        for (int i = lastNum - 1; i > 0; i--)
        {
            throwTime++;
            // ���� Ž�� ���� x, y ��ġ ���� ���� ��ġ ������ �۴ٸ�
            if (posList[i - 1].x > posList[i].x && posList[i - 1].y > posList[i].y)
            {
                // ���� �ۿ�Ǵ� �������� ���Ѵ�.
                mPos = posList[i];
                break;
            }
        }

        posList.Clear();
        return mPos;
    }

    // �� ���ϻ��� ��ġ�� �ڷ�ƾ �Լ�
    private IEnumerator ReleaseChichute()
    {
        // �� ���ϻ� ��� Ƚ���� �����Ѵ�.
        chichuteCount--;
        // �� ���ϻ� ������Ʈ�� ���̰� ����
        chichute.SetActive(true);
        float velocity_y = 0f;  // y�� �ӵ�

        AudioManager.Instance.chicuteSound.Play();

        // 400������ ���� �����Ѵ�.
        for (int i = 0; i < 400; i++)
        {
            // ���� �÷��̾��� y�� �ӵ��� ���� ���ҽ�Ų��.
            velocity_y = playerRb2D.velocity.y * 0.98f;
            // x�� �������� ���� ���ݾ� ���� ������ �ش�.
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
