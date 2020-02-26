using UnityEngine;

[CreateAssetMenu(fileName = "BasicMovementDataObject", menuName = "Player/BasicMovementDataObject")]
public class BasicMovementData : ScriptableObject
{
    public float maxMovementSpeed;
    public float movementForce;
    public float jumpForce;
    public LayerMask groundLayers;
}
