using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour
{
    public bool isMoving;       // �̵��ϰ� �ִ���
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public int isFull;              // ����Ʈ �䱸���� ����
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��
    public int[] getDesserts;   // ���� ����/����ũ
    private NavMeshAgent navMesh;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        isMoving = true;
        isFull = 0;
        InitTakeOut();
    }
    private void Update()
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)
        {
            GameManager.instance.customerMoving.CarsShiftForward();
            isFull++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            for (int i = 0; i < getDesserts.Length; i++)
            {
                getDesserts[i] = 0;
            }
            gameObject.SetActive(false);
        }
    }
    void InitTakeOut()     // ����̺꽺�� �ֹ��� 0~4�ε� �Ѵ� 0�̸� ����
    {
        isRequesting = false;
        if (GameManager.instance.upgradeScript.stoveLevel < 3)
        {
            requires[0] = Random.Range(1, 5);
        }
        else if (GameManager.instance.upgradeScript.stoveLevel >= 3)
        {
            requires = requires.Select(x => Random.Range(1, 5)).ToArray();
        }
        if (requires[0] <= 0 && requires[1] <= 0) InitTakeOut();
    }
}
