using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShower : MonoBehaviour
{
    [SerializeField] private List<Image> _hearts = new();

    public void OnAmountChange(int value)
    {
        for (var i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].color = i < value
                ? Color.white
                : Color.black;
        }
    }
}