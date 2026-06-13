using UnityEngine;

public class WaistSocketFollow : MonoBehaviour
{
    [Header("Referência da cabeça do jogador")]
    [SerializeField] private Transform head;

    [Header("Posição do socket em relação ao jogador")]
    [SerializeField] private Vector3 localOffset = new Vector3(0.25f, -0.65f, 0.15f);

    [Header("Rotação extra do socket")]
    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

    private void LateUpdate()
    {
        if (head == null)
            return;

        Vector3 flatForward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;

        if (flatForward.sqrMagnitude < 0.001f)
            return;

        Quaternion bodyYaw = Quaternion.LookRotation(flatForward, Vector3.up);

        transform.position = head.position + bodyYaw * localOffset;
        transform.rotation = bodyYaw * Quaternion.Euler(rotationOffset);
    }
}