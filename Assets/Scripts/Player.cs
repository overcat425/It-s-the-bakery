using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    float hor;
    float ver;
    public float speed;
    //bool runKey;
    bool isCarrying;
    bool[] isTuto = new bool[2];  // 0�� Stove 1�� Cust
    Vector3 moveVec;
    float camY;
    Animator anim;

    //public Queue<int> playerQueue = new Queue<int>(); // �����غ��ϱ� ���� ť�� ��ߵǴ°�...??????????
    public int[] playerDesserts = { 0, 0 };  // 0�� Donut, 1�� Cake
    [SerializeField] GameObject[] donutsPrefab;
    [SerializeField] GameObject[] cakePrefab;
    public int maxPlayerDesserts = 5;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine("PlayerDesserts");
    }
    void Update()
    {
        Inputs();
        Move();
    }
    private void LateUpdate()
    {
        DessertsUi(playerDesserts, donutsPrefab, cakePrefab);
    }
    IEnumerator PlayerDesserts()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
        }
    }
    void Inputs()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
        //runKey = Input.GetButton("Run");
        //jumpKey = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        camY = Camera.main.transform.eulerAngles.y;     // ī�޶� -45 ȸ���� ���� �ذ�å
        Quaternion camRot = Quaternion.Euler(0f, camY, 0f);     // ī�޶��� Y�ุ ���� ��,
        speed = (2 + GameManager.instance.upgradeScript.moveSpeed * 0.3f);
        moveVec = camRot *  new Vector3(hor, 0, ver).normalized;            // �÷��̾��� �̵����� ������
        transform.position += moveVec * speed * Time.deltaTime;
        isCarrying = playerDesserts[0] == 0 && playerDesserts[1]==0 ? false : true;
        if (!isCarrying)
        {
            anim.SetBool("isCarry", false); anim.SetBool("isCarryMove", false);
            anim.SetBool("isWalk", moveVec != Vector3.zero);
        }else if (isCarrying)
        {
            anim.SetBool("isCarry", moveVec == Vector3.zero);
            anim.SetBool("isCarryMove", moveVec != Vector3.zero);
        }
        //if (moveVec != Vector3.zero)anim.SetBool("isRun", runKey);
        //if (moveVec == Vector3.zero) anim.SetBool("isRun", false);
        transform.LookAt(moveVec+transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        StoveScript stoveScript = collision.gameObject.GetComponent<StoveScript>();
        switch (collision.gameObject.tag)
        {
            case "Stove":
                if (stoveScript.stoveDesserts > 0)
                {
                    Tuto(0);
                    while (playerDesserts[0] < maxPlayerDesserts)
                    {
                        stoveScript.stoveDesserts--; playerDesserts[0]++;
                        if (stoveScript.stoveDesserts <= 0) break;
                    }
                }
                break;
            case "CakeStove":
                if (stoveScript.stoveDesserts > 0)
                {
                    if (stoveScript.stoveDesserts > 0)
                    {
                        while (playerDesserts[1] < maxPlayerDesserts)
                        {
                            stoveScript.stoveDesserts--; playerDesserts[1]++;
                            if (stoveScript.stoveDesserts <= 0) break;
                        }
                    }
                }
                break;
            case "Trash":
                for (int i = 0; i < maxPlayerDesserts; i++)
                {
                    donutsPrefab[i].SetActive(false);
                    cakePrefab[i].SetActive(false);
                    if (i < 2) playerDesserts[i] = 0;
                }
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance.customerMoving.customerObjects.Count <= 0) return;
        if (other.gameObject.CompareTag("Counter")){
            CustomerScript customerScript = GameManager.instance.customerMoving.customerObjects[0].GetComponent<CustomerScript>();
            for (int i = 0; i < playerDesserts.Length; i++)
            {
                int req = customerScript.requires[i];
                if (req <= customerScript.getDesserts[i] || customerScript.isRequesting == false) continue;
                while (playerDesserts[i] > 0)
                {
                    switch (i)
                    {
                        case 0:
                            donutsPrefab[playerDesserts[i] - 1].SetActive(false);
                            GameManager.instance.GetMoney(GameManager.instance.donutCost);
                            break;
                        case 1:
                            cakePrefab[playerDesserts[i] - 1].SetActive(false);
                            GameManager.instance.GetMoney(GameManager.instance.cakeCost);
                            break;
                    }
                    playerDesserts[i]--;
                    customerScript.getDesserts[i]++;
                    if(req <= customerScript.getDesserts[i]) break;
                }Tuto(1);
                GameManager.instance.upgradeScript.DisableBtn();
            }
        }
    }
    public void DessertsUi(int[] desserts, GameObject[] donuts, GameObject[] cake)
    {
        for(int i = 0; i < desserts.Length; i++)
        {
            for (int j = 0; j < desserts[i]; j++)
            {
                switch (i)
                {
                    case 0:
                        donuts[j].SetActive(true);
                    break;
                        case 1:
                        cake[j].SetActive(true);
                    break;
                }
            }
        }
    }
    void IsCarrying()
    {
        for(int i = 0; i < playerDesserts.Length; i++)
        {
            if (playerDesserts[i] > 0) isCarrying = true;
        }
        if (playerDesserts[0] == 0 && playerDesserts[1]==0) isCarrying= false;
    }
    void Tuto(int i)
    {
        if (isTuto[i] == false)
        {
            GameManager.instance.tutorialScript.NextPosition();
            GameManager.instance.textScript.ShowNextText();
            isTuto[i] = true;
        }
    }
}