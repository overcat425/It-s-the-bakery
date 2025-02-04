using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerScript : MonoBehaviour
{
    //public Transform homeTrans;
    public bool isMoving;       // �̵��ϰ� �ִ���
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public bool isEating;       // �¼��� �����ؼ� �Ա� ����
    public int isFull;              // ����Ʈ �䱸���� ����
    bool isCarrying;            // ����Ʈ ����ִ��� �ִϸ��̼ǿ���
    bool isGetting;
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��
    public int[] getDesserts;   // ���� ����/����ũ
    public Transform[] customerBaskets;
    //[SerializeField] GameObject[] donutsPrefab;
    //[SerializeField] GameObject[] cakePrefab;
    public int eatingTime;
    private NavMeshAgent navMesh;   // �̵����
    Rigidbody rigid;
    public Animator anim;

    float timer;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isEating = false;
        isMoving = true;
        isFull = 0;
        InitRequire();
        eatingTime = Random.Range(25, 35);
    }
    void Update()
    {
        MoveOrNot();
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)
        {
            //StartCoroutine("HoldDesserts");
            GameManager.instance.customerMoving.ShiftObjectsForward();
            isFull++;
            SoundManager.instance.PlaySound(SoundManager.Effect.Counter);
            //GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.seats[0]);//destroyPoint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Seat")&&isEating==false)
        {
            StartCoroutine("EatingTime");
            isEating= true;
        }
    }
    public void MoveOrNot()     // �ȱ�,���� �ִϸ��̼� ��Ʈ��
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        anim.SetBool("isWalk", isMoving);
    }
    void InitRequire()     // ��ǰ �䱸�� �޼ҵ� ; 0~3�ε� �Ѵ� 0�̸� ����
    {
        isRequesting = false;
        if (GameManager.instance.upgradeScript.stoveLevel < 3)
        {
            requires[0] = Random.Range(1, 4);
        }
        else if (GameManager.instance.upgradeScript.stoveLevel >= 3)
        {
            requires = requires.Select(x => Random.Range(1, 4)).ToArray();
        }
        if (requires[0] <= 0 && requires[1]<=0) InitRequire();
    }
    IEnumerator EatingTime()        // �Դ� ����
    {                                                                             // ���� �ڸ���ȣ ����
        int index = GameManager.instance.customerMoving.seatObjects.IndexOf(gameObject);
        LookAtTable(index);//SitOn(index);
        yield return StartCoroutine("SitDown"); // �ɴ� �ִϸ��̼�
        anim.SetBool("isEating", true);         // �Դ� �ִϸ��̼� ����
        navMesh.isStopped = true;
        yield return new WaitForSeconds(eatingTime);    //�Դ� �ð�
        yield return StartCoroutine("StandUp");  // �Ͼ�� �ִϸ��̼�
        anim.SetBool("isEating", false);        // �Դ� �ִϸ��̼� ��
        GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.destroyPoint); // �մ� ����
        GameManager.instance.customerMoving.seatObjects[index] = null;
        for (int i = 0; i < getDesserts.Length; i++)
        {
            getDesserts[i] = 0;
        }
        navMesh.isStopped = false;              // �Դ� ������ �̵� �Ұ�
    }
    IEnumerator SitDown()
    {
        anim.SetTrigger("sit");
        yield return WaitForAnimation("StandToSit", 0.8f);
    }
    IEnumerator StandUp()
    {
        anim.SetTrigger("stand");
        yield return WaitForAnimation("SitToStand", 1f);
    }
    IEnumerator WaitForAnimation(string animation, float duration)
    {
        yield return new WaitUntil(() => //�ִϸ��̼� ���� ������ ��ٸ��� �޼ҵ�
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            return state.IsName(animation) && state.normalizedTime >= duration;
        });
    }
    //IEnumerator HoldDesserts()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    GameManager.instance.player.DessertsUi(getDesserts, donutsPrefab, cakePrefab);
    //}
    //void SitOn(int i)
    //{
    //    for (int k = 0; k < getDesserts.Length; k++)
    //    {
    //        for (int j = 0; j < getDesserts[k]; j++)
    //        {
    //            switch (k)
    //            {
    //                case 0:
    //                    donutsPrefab[j].SetActive(false);
    //                    break;
    //                case 1:
    //                    cakePrefab[j].SetActive(false);
    //                    break;
    //            }
    //        }
    //    }
    //    LookAtTable(i);
    //}
    void LookAtTable(int i)
    {
        switch (i%2)
        {
            case 0:
                gameObject.transform.LookAt(GameManager.instance.customerMoving.turn[0]);
                break;
            case 1:
                gameObject.transform.LookAt(GameManager.instance.customerMoving.turn[1]);
                break;
        }
    }
}