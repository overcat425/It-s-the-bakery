using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour
{
    public GameObject requireBurgers;   // �մ��� �䱸�ϴ� ���� Ui
    public GameObject[] burgerImg;      // �մ��� �䱸�ϴ� ���� �� �̹���
    public int require;
    private void Awake()
    {
        requireBurgers.SetActive(true);
    }
    private void Update()
    {
        if(GameManager.instance.customerObjects.Count >= 1)IsFirstObject();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            RefreshRequires();
        }
    }
    void IsFirstObject()        // ���� �� �մ��� �ܹ��� �䱸�� ����
    {
        GameObject yourTurn = GameManager.instance.customerObjects[0];
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();
        if (requireBurgers.activeSelf)
        {
            requireBurgers.transform.position = Camera.main.WorldToScreenPoint(yourTurn.transform.position + new Vector3(0, 3f, 0));
            for (int i = 0; i < customerScript.burgerRequire; i++)
            {
                burgerImg[i].SetActive(true);
            }
        }
    }
    public void RefreshRequires()
    {
        for (int i = 0; i < 5; i++)
        {
            burgerImg[i].SetActive(false);
        }
    }
}
