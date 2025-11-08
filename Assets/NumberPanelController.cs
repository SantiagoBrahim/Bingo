using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPanelController : MonoBehaviour
{
    public GameObject number;
    // Start is called before the first frame update
    void Start()
    {
        int n = 1;
        while (n <= 90)
        {
            GameObject numberInstance = Instantiate(number, gameObject.transform);
            numberInstance.name = n.ToString();
            numberInstance.GetComponentInChildren<TextMeshProUGUI>().text = n.ToString();
            n++;
        }
    }

    public void UpdatePanel(int numberToUpdate)
    {
        GameObject child = GameObject.Find(numberToUpdate.ToString());
        if (child != null)
        {
            child.GetComponent<Image>().color = Color.red;
            child.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            child.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
    }

    public void ClearPanel()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.GetComponent<Image>().color = Color.white;
            child.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            child.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.LowerCase;
        }
    }
}
