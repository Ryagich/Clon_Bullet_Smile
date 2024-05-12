using UnityEngine;

public class Target : MonoBehaviour
{
    public static Target Instance;
    [field: SerializeField] public Health Health;
    
    private void Awake()
    {
        Instance = this;
    }
}
