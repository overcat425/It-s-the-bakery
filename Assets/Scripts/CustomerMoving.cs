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
    private void Start()
    {
        StartCoroutine("CustomersComing");
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
        if (customerObjects.Count > 0)
        {
            GameObject firstObject = customerObjects[0];
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
        }
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
}
