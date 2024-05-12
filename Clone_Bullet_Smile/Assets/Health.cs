using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _amount;

    public void ChangeAmount(int value)
    {
        _amount += value;
        Debug.Log(_amount);
    }
}
