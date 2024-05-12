using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShower : MonoBehaviour
{
    [SerializeField] private List<Image> _hearts = new();

    public void OnAmountChange(int value)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            if (i < value)
            {
                _hearts[i].color = Color.white;
            }
            else
            {
                _hearts[i].color = Color.black;
            }
        }
    }
}