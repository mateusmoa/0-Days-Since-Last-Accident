using UnityEngine;

/// <summary>
/// Faz o menu acompanhar o jogador suavemente no VR.
/// Coloque este script no objeto-pai do menu (o Canvas em World Space).
/// </summary>
public class MenuFollow : MonoBehaviour
{
    [Header("Referencia")]
    [Tooltip("A camera do VR. Geralmente a Main Camera dentro do XR Origin.")]
    public Transform cabeca;

    [Header("Posicionamento")]
    [Tooltip("Distancia do menu a frente do jogador, em metros.")]
    public float distancia = 2f;

    [Tooltip("Altura em relacao aos olhos. Negativo deixa o menu um pouco abaixo da linha de visao.")]
    public float alturaRelativa = -0.2f;

    [Header("Suavizacao (quanto maior, mais rapido cola no alvo)")]
    [Tooltip("Velocidade de reposicionamento. Valores baixos = movimento mais suave/preguicoso.")]
    public float suavidadeMovimento = 4f;

    [Tooltip("Velocidade de rotacao para encarar o jogador.")]
    public float suavidadeRotacao = 6f;

    void Start()
    {
        // Se voce nao arrastar a camera no Inspector, tenta achar sozinho.
        if (cabeca == null && Camera.main != null)
            cabeca = Camera.main.transform;

        // Posiciona ja no lugar certo no primeiro frame (evita o menu "voar" da origem ate voce).
        if (cabeca != null)
        {
            transform.position = PosicaoAlvo();
            transform.rotation = RotacaoAlvo();
        }
    }

    void LateUpdate()
    {
        // LateUpdate roda DEPOIS que o tracking move a camera no frame,
        // entao o menu sempre usa a posicao mais recente da sua cabeca.
        if (cabeca == null) return;

        transform.position = Vector3.Lerp(
            transform.position, PosicaoAlvo(), suavidadeMovimento * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, RotacaoAlvo(), suavidadeRotacao * Time.deltaTime);
    }

    // Ponto a frente do jogador, na altura escolhida.
    Vector3 PosicaoAlvo()
    {
        // Pega para onde a cabeca aponta, mas zera a inclinacao vertical
        // para o menu nao subir/descer quando voce olha pra cima ou pra baixo.
        Vector3 frente = cabeca.forward;
        frente.y = 0f;
        frente.Normalize();

        return cabeca.position + frente * distancia + Vector3.up * alturaRelativa;
    }

    // Rotacao que deixa a frente do Canvas legivel para o jogador.
    Quaternion RotacaoAlvo()
    {
        Vector3 direcao = transform.position - cabeca.position; // aponta pra longe da cabeca
        direcao.y = 0f;
        if (direcao.sqrMagnitude < 0.0001f) return transform.rotation;
        return Quaternion.LookRotation(direcao);

        // Se o menu aparecer ESPELHADO/de costas pra voce, troque a linha de cima por:
        // return Quaternion.LookRotation(cabeca.position - transform.position);
    }
}
