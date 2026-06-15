using System.Collections;
using UnityEngine;

/// <summary>
/// Tremida de câmera para VR.
///
/// ⚠️ Em VR, balançar a câmera pode dar enjoo. Mantenha pequeno e curto.
///
/// ONDE COLOCAR (recomendado): direto no "Camera Offset" do XR Origin.
///   Não precisa reparentar nada nem criar pivot — assim você NÃO mexe na
///   estrutura do prefab do XR Origin (evita o aviso de "restructuring").
///   Funciona porque o tracking escreve na Main Camera, e este script escreve
///   no Camera Offset (o pai), então os dois se somam sem brigar.
///
///   (Só crie um pivot vazio entre o Offset e a Main Camera se você tiver algum
///    sistema próprio que mexa na posição do Camera Offset TODO FRAME.)
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Tooltip("Transform que vai tremer. Se vazio, usa o próprio (coloque no Camera Offset).")]
    [SerializeField] private Transform shakeTarget;

    [Header("Valores padrão")]
    [SerializeField] private float defaultDuration = 0.15f;
    [Tooltip("Deslocamento máximo em metros. MANTENHA PEQUENO! (0.01 a 0.05)")]
    [SerializeField] private float defaultMagnitude = 0.03f;

    private Vector3 restLocalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        if (shakeTarget == null) shakeTarget = transform;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Shake() => Shake(defaultDuration, defaultMagnitude);

    public void Shake(float duration, float magnitude)
    {
        if (shakeTarget == null) return;

        // Se já estava tremendo, volta pro lugar antes de recapturar a base
        // (pra não acumular offset em cima de offset).
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            shakeTarget.localPosition = restLocalPos;
        }

        // Captura a posição de descanso AGORA: robusto mesmo que o XR Origin
        // tenha ajustado a altura do Camera Offset no começo do jogo.
        restLocalPos = shakeTarget.localPosition;
        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // A intensidade cai até zero pra terminar suave em vez de "cortar".
            float strength = magnitude * (1f - (elapsed / duration));
            shakeTarget.localPosition = restLocalPos + Random.insideUnitSphere * strength;
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeTarget.localPosition = restLocalPos;
        shakeRoutine = null;
    }
}