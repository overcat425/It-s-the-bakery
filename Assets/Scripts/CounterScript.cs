using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    public GameObject requireInven;   // �մ��� �䱸�ϴ� ��ǰ Ui(��ü������)
    public GameObject[] requireUi;      // �մ��� �䱸�ϴ� ��ǰ��Ui
    public Text[] requireText;
    private void Update()
    {
        if(GameManager.instance.customerObjects.Count >= 1)IsFirstObject();
    }
    void IsFirstObject()        // ���� �� �մ��� ��ǰ �䱸�� ����
    {
        GameObject yourTurn = GameManager.instance.customerObjects[0];
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();
        requireInven.transform.position = Camera.main.WorldToScreenPoint(yourTurn.transform.position + new Vector3(0, 3f, 0));
        if (customerScript.isMoving == false)
        {
            requireInven.SetActive(true);
            customerScript.isRequesting = true;
            for (int k = 0; k < requireUi.Length; k++)
            {
                bool isUiHide = customerScript.requires[k] > 0 ? false : true;
                requireUi[k].gameObject.SetActive(!isUiHide);
            }
            for (int i = 0; i < requireText.Length; i++)
            {
                requireText[i].text = string.Format("{0}", customerScript.requires[i]);
            }
        }
        else requireInven.SetActive(false);
    }
}