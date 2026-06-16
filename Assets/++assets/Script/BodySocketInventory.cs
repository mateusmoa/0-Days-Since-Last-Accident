using UnityEngine;

[System.Serializable]
public class bodySocket
{
    public GameObject gameObject;
    [Range(0.01f, 1f)]
    public float heightRatio;
}

public class BodySocketInventory : MonoBehaviour
{
    [SerializeField] private GameObject HMD;          // a Main Camera (capacete)
    [SerializeField] private bodySocket[] bodySockets;

    private Transform _hmd;

    void Awake()
    {
        if (HMD != null) _hmd = HMD.transform;
    }

    // LateUpdate: lê a pose da cabeça DEPOIS do tracking atualizar (segue sem atraso)
    void LateUpdate()
    {
        if (_hmd == null) return;

        Vector3 headLocal = _hmd.localPosition;

        // posição: fica embaixo da cabeça (x,z), no chão do rig (y = 0)
        transform.localPosition = new Vector3(headLocal.x, 0f, headLocal.z);

        // rotação: só o yaw (pra onde a cabeça olha no plano), sempre em pé.
        // Calculado do zero todo frame, então NÃO acumula erro.
        Vector3 fwd = _hmd.forward;
        fwd.y = 0f;
        if (fwd.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(fwd.normalized, Vector3.up);

        // altura de cada socket, proporcional à altura da cabeça (mantém o x/z que você posicionou)
        float headHeight = headLocal.y;
        foreach (var socket in bodySockets)
        {
            if (socket == null || socket.gameObject == null) continue;
            Vector3 p = socket.gameObject.transform.localPosition;
            socket.gameObject.transform.localPosition =
                new Vector3(p.x, headHeight * socket.heightRatio, p.z);
        }
    }
}
