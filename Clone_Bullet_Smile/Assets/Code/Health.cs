using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Died;
    public UnityEvent<int> AmountChanged;

    public bool Alive { get; private set; } = true;
    [SerializeField] [Min(1)] private int _amount;

    private int amount;

    private void Start()
    {
        amount = _amount;
    }

    public void ChangeAmount(int value)
    {
        amount = Mathf.Clamp(amount + value, 0, _amount);
        if (amount <= 0 && Alive)
        {
            Alive = false;
            Died?.Invoke();
        }

        AmountChanged?.Invoke(amount);
    }
}