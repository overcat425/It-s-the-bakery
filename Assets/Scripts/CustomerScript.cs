using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerScript : MonoBehaviour
{
    public Transform homeTrans;
    public bool isMoving;
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��

    private NavMeshAgent navMesh;
    Rigidbody rigid;
    public Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isMoving = true;
        InitRequire();
    }
    void Update()
    {
        MoveOrNot();
        if (requires[0] <= 0 && requires[1]<=0)
        {
            GameManager.instance.customerMoving.ShiftObjectsForward();
            requires[0]++; // �Ѵ� 00�� ���¸� ����Ʈ�� �ִ°� �ϴ� ������..
            GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.destroyPoint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
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
        if (requires[0] == 0 && requires[1]==0) InitRequire();
    }
}