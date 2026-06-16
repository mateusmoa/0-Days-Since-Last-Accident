using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    [SerializeField] private Transform checkpointInicial; // onde o player começa

    void Awake() => Instance = this;

    void Start()
    {
        // garante um respawn válido antes de bater o ponto a 1ª vez
        if (checkpointInicial != null) SetCheckpoint(checkpointInicial);
    }

    public void SetCheckpoint(Transform t)
    {
        Position = t.position;
        Rotation = t.rotation;
    }
}
