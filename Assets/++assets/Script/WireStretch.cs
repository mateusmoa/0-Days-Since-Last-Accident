using UnityEngine;

// =======================================================================
// Versão VR do ScaleWithPivots. Estica o cilindro do fio entre dois pontos.
//
// Mesma matemática do original, com 2 diferenças:
//   1) Atualiza TODO frame (LateUpdate) em vez de depender de transform.hasChanged
//      — porque no VR a ponta se move continuamente enquanto está na mão.
//   2) Roda em LateUpdate pra acontecer DEPOIS do XR mover a ponta no frame.
//
// Vai no cilindro do fio.
//   start = ponto fixo (âncora) de onde o fio sai
//   end   = a ponta móvel (o WireConnector)
// =======================================================================
public class WireStretch : MonoBehaviour
{
    public Transform start; // âncora fixa (lado de origem do fio)
    public Transform end;   // ponta móvel (WireConnector)

    private Vector3 _initialScale;

    private void Start() => _initialScale = transform.localScale;

    private void LateUpdate()
    {
        if (start == null || end == null) return;

        float distance = Vector3.Distance(start.position, end.position);

        // posição = ponto médio entre âncora e ponta
        transform.position = (start.position + end.position) * 0.5f;

        // rotação = aponta da âncora para a ponta
        transform.up = end.position - start.position;

        // escala no eixo Y; divide por 2 porque o cilindro do Unity
        // cresce pros dois lados a partir do centro
        transform.localScale = new Vector3(_initialScale.x, distance * 0.5f, _initialScale.z);
    }
}
