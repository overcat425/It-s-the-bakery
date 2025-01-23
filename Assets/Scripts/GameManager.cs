using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CustomerScript customerScript;
    public CustomersPool customersPool;
    public CounterScript counterScript;
    public Player player;

    [SerializeField] ItemData[] itemData;
    [SerializeField] Text moneyText;
    public int money;
    public int donutCost;
    public int cakeCost;
    

    [SerializeField] Transform spawnPoint;
    public List<Transform> counters = new List<Transform>();
    public List <GameObject> customerObjects = new List <GameObject>();
    private void Awake()        // �̱���
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        Costing();
    }
    void Start()
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
            firstObject.SetActive(false);
            for (int i = 0; i < customerObjects.Count; i++)
            {                   //  �������� �� ĭ�� �̵�(��ġ)
                Destination(customerObjects[i], counters[i]);
            }
            if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
            {           // �������� �� ĭ�� �̵�(����Ʈ��ȣ)
                Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count-1]);
            }
        }
    }
    IEnumerator CustomersComing()       // �մ� ���� �޼ҵ�
    {
        while (true)
        {
            float rand = Random.Range(3, 9);        // 3~8�� ������Ÿ��
            yield return new WaitForSeconds(rand);
            if(customerObjects.Count < 8)
            {
                GameObject enemy = customersPool.MakeBugy(0);
                enemy.transform.position = spawnPoint.position;
                customerObjects.Add(enemy);
                Destination(enemy, counters[customerObjects.Count-1]);
            }
        }
    }
    public void GetMoney(int cost)      // �մ� ��굵�͵帮�����ϴ�
    {
        money += cost;
        moneyText.text = string.Format("{0}", money);
    }
    void Costing()      // ������ + ��ǰ�� �ݾ� �ʱ�ȭ
    {
        money = 0;
        for (int i = 0; i < itemData.Length; i++)
        {
            switch (itemData[i].itemType)
            {
                case ItemData.ItemType.Doughnut:
                    donutCost = 50;
                    break;
                case ItemData.ItemType.Cake:
                    cakeCost = 100;
                    break;
            }
        }
    }
}
