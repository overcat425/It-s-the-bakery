using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerMoving : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    public Transform destroyPoint;
    public List<Transform> counters = new List<Transform>();
    public List<GameObject> customerObjects = new List<GameObject>();
    public List<Transform> seats = new List<Transform>();
    public List<GameObject> seatObjects = new List<GameObject>();

    public Transform[] turn;
    [SerializeField] GameObject noSeat;

    private void OnEnable()
    {
        StartCoroutine("CustomersComing");
    }
    private void LateUpdate()
    {
        noSeat.transform.position = Camera.main.WorldToScreenPoint(counters[0].transform.position + new Vector3(0, 2f, 0));
    }
    public void Destination(GameObject cust, Transform dest)
    {                                       // �մԵ� ������ ����
        NavMeshAgent agent = cust.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(dest.position);
        }
    }
    public void ShiftObjectsForward()    // �մԵ� ������ ��ĭ�� �̵�
    {
        if (customerObjects.Count > 0&& IsThereSeat()>0 )
        {
            noSeat.SetActive(false);
            GameObject firstObject = customerObjects[0];
            FindSeat(firstObject);
            customerObjects.RemoveAt(0);  // ù ��° ������Ʈ�� ����Ʈ���� ����
            //firstObject.SetActive(false);
            for (int i = 0; i < customerObjects.Count; i++)
            {                   //  �������� �� ĭ�� �̵�(��ġ)
                Destination(customerObjects[i], counters[i]);
            }
            if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
            {           // �������� �� ĭ�� �̵�(����Ʈ��ȣ)
                Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count - 1]);
            }
        }else if(IsThereSeat() <= 0) {
            StartCoroutine("NoSeat");
        }
    }
    public void FindSeat(GameObject cust)
    {
        for (int i = 0; i < seats.Count; i++)
        {
            if (seatObjects[i] == null)
            {
                seatObjects[i] = cust;
                Destination(seatObjects[i], seats[i]);
                break;
            }
        }
    }
    int IsThereSeat()
    {
        int seat = 0;
        for (int i = 0; i < seats.Count; i++)
        {
            if (seatObjects[i] == null) seat++;
        }return seat;
    }
    IEnumerator CustomersComing()       // �մ� ���� �޼ҵ�
    {
        while (true)
        {
            float rand = Random.Range(3, 7);        // 3~6�� ������Ÿ��
            yield return new WaitForSeconds(rand);
            if (customerObjects.Count < 8)
            {
                GameObject enemy = GameManager.instance.customersPool.MakeBugy(0);
                enemy.transform.position = spawnPoint.position;
                customerObjects.Add(enemy);
                Destination(enemy, counters[customerObjects.Count - 1]);
            }
        }
    }
    IEnumerator NoSeat()
    {
        noSeat.SetActive(true);
        yield return new WaitForSeconds(3f);
        ShiftObjectsForward();
    }
}
