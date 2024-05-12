using UnityEngine;

[CreateAssetMenu(menuName = "TurretSettings")]
public class TurretSettings : ScriptableObject
{
    [field: SerializeField, Range(.0f, 50f)] public float Radius { get; private set; }
    [field: SerializeField, Range(.0f, 360f)] public float FieldAngle { get; private set; }
    [field: SerializeField, Range(.0f, 1)] public float FOVCheckTime { get; private set; }
    [field: SerializeField, Range(1, 120)] public int FOVLineCount { get; private set; }
    
    [field: SerializeField, Min(0)] public float OffsetToRotation{ get; private set; }
    [field: SerializeField] public float TimeToLoseTarget{ get; private set; }
    [field: SerializeField] public float RotateHoldTime{ get; private set; }
    [field: SerializeField, Min(0)] public float RotateSpeed { get; private set; } = 2f;
    
    [field: SerializeField, Range(.0f, 360f)] public float ShootAngle { get; private set; }
    [field: SerializeField, Range(.0f, 10)] public float ShootTime { get; private set; }
    [field: SerializeField, Range(.0f, 10)] public float ShootCheckTime { get; private set; }
    [field: SerializeField, Range(0, 3)] public int Damage { get; private set; }

}