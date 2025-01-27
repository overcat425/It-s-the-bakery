using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("�ν��Ͻ�")]
    public static GameManager instance;
    public UpgradeScript upgradeScript;
    public CustomerScript customerScript;
    public CustomersPool customersPool;
    public CustomerMoving customerMoving;
    public CounterScript counterScript;
    public CameraManager cameraManager;
    public Player player;

    [SerializeField] ItemData[] itemData;
    [SerializeField] Text moneyText;
    public int money;
    public int donutCost;
    public int cakeCost;

    [Header("�Ͻ����� UI")]
    [SerializeField] GameObject pauseUi;
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
    public void GetMoney(int cost)      // �մ� ��굵�͵帮�����ϴ�
    {
        money += cost;
        MoneySync();
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
    public void MoneySync()
    {
        moneyText.text = string.Format("{0}", money);
    }
    public void PauseUiToggle()
    {
        if (pauseUi.activeSelf) { pauseUi.SetActive(false);
        }else if (pauseUi.activeSelf==false) pauseUi.SetActive(true);
    }
}