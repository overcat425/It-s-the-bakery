using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour  // �մ� �䱸������ �ô� ��ũ��Ʈ(���X)
{
    public enum CounterType { Counter, Thru }
    public CounterType counterType;
    public GameObject[] requireInven;   // �մ��� �䱸�ϴ� ��ǰ Ui(��ü������)
    public GameObject[] requireUi;      // �մ��� �䱸�ϴ� ��ǰ��Ui
    public Text[] requireText;

    public GameObject[] carUi;
    public Text[] carText;
    private void Update()
    {
        Selling();
    }
    void Selling()
    {
        CustomerMoving customerMoving = GameManager.instance.customerMoving;
        switch (counterType)
        {
            case CounterType.Counter:
                if (customerMoving.customerObjects.Count >= 1) IsFirstObject();
                break;
            case CounterType.Thru:
                if (customerMoving.carObjects.Count >= 1) IsFirstCar();
                break;
        }
    }
    void IsFirstObject()        // ���� �� �մ��� ��ǰ �䱸�� ����
    {
        GameObject yourTurn = GameManager.instance.customerMoving.customerObjects[0];
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();
        requireInven[0].transform.position = Camera.main.WorldToScreenPoint(yourTurn.transform.position + new Vector3(0, 3f, 0));
        if (customerScript.isMoving == false)
        {
            requireInven[0].SetActive(true);
            customerScript.isRequesting = true;
            for (int k = 0; k < requireUi.Length; k++)
            {
                bool isUiHide = customerScript.requires[k] - customerScript.getDesserts[k] > 0 ? false : true;
                requireUi[k].SetActive(!isUiHide);
            }
            for (int i = 0; i < requireText.Length; i++)
            {
                requireText[i].text = string.Format("{0}", customerScript.requires[i] - customerScript.getDesserts[i]);
            }
        }
        else requireInven[0].SetActive(false);
    }
    void IsFirstCar()
    {
        GameObject firstCar = GameManager.instance.customerMoving.carObjects[0];
        CarScript carScript = firstCar.GetComponent<CarScript>();
        requireInven[1].transform.position = Camera.main.WorldToScreenPoint(firstCar.transform.position + new Vector3(0, 3f, 1f));
        if (carScript.isMoving == false)
        {
            requireInven[1].SetActive(true);
            carScript.isRequesting = true;
            for (int k = 0; k < carUi.Length; k++)
            {
                bool isUiHide = carScript.requires[k] - carScript.getDesserts[k] > 0 ? false : true;
                carUi[k].SetActive(!isUiHide);
            }
            for (int i = 0; i < carText.Length; i++)
            {
                carText[i].text = string.Format("{0}", carScript.requires[i] - carScript.getDesserts[i]);
            }
        }
        else requireInven[1].SetActive(false);
    }
}