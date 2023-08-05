using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueDisplayUI : MonoBehaviour
{
    [SerializeField] private GameObject valueDisplayUI;

    public void DisplayValue(float value, Color color)
    {
        var valueUI = Instantiate(valueDisplayUI, transform.position, Quaternion.identity);
        valueUI.GetComponent<TextMeshPro>().text = value.ToString();
        valueUI.GetComponent<TextMeshPro>().color = color;
    }
}
