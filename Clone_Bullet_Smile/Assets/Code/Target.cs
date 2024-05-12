using UnityEngine;

public class Target : MonoBehaviour
{
    public static Target Instance;
    
    [field: SerializeField] public Health Health { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}