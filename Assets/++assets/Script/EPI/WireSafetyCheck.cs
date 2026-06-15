using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

/// <summary>
/// Coloque junto ao XR Grab Interactable de cada FIO.
/// Verifica se o jogador está com a luva:
///   - Com luva  -> deixa pegar normalmente.
///   - Sem luva  -> dá feedback (vibração + som + tremida) e, se quiser, BLOQUEIA o grab.
///
/// Funciona como um "Select Filter" do XRI: o toolkit consulta este script ANTES de
/// deixar pegar o fio, então dá pra recusar o grab de forma limpa (sem cancelar na mão).
/// </summary>
public class WireSafetyCheck : MonoBehaviour, IXRSelectFilter
{
    [Header("Comportamento")]
    [Tooltip("Ligado: sem luva o fio NÃO pode ser pego (só toma o choque). " +
             "Desligado: pega mesmo assim, mas leva o feedback do mesmo jeito.")]
    [SerializeField] private bool blockGrabWithoutGlove = true;

    [Tooltip("Tempo mínimo entre um feedback e outro, em segundos (evita spam de vibração " +
             "enquanto a pessoa segura o grip em cima do fio).")]
    [SerializeField] private float feedbackCooldown = 0.5f;

    [Header("1) Vibração do controle")]
    [Range(0f, 1f)][SerializeField] private float hapticAmplitude = 0.8f;
    [SerializeField] private float hapticDuration = 0.25f;

    [Header("2) Som de choque")]
    [SerializeField] private AudioClip shockSound;
    [Tooltip("Opcional. Se vazio, toca o som na posição do fio (PlayClipAtPoint).")]
    [SerializeField] private AudioSource audioSource;

    [Header("3) Tremida de câmera")]
    [SerializeField] private bool useCameraShake = true;
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeMagnitude = 0.03f;

    [Header("Extra")]
    [Tooltip("Disparado quando alguém encosta no fio sem luva. " +
             "Ligue aqui o método do seu contador de acidentes, por exemplo.")]
    [SerializeField] private UnityEvent onUnsafeContact;

    // --- IXRSelectFilter ---
    // Enquanto for true, este filtro participa da validação do grab.
    public bool canProcess => isActiveAndEnabled;

    private XRBaseInteractable interactable;
    private float lastFeedbackTime = -999f;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
            Debug.LogError("[WireSafetyCheck] Precisa de um XR Grab Interactable (ou outro XRBaseInteractable) no mesmo objeto.", this);
    }

    private void OnEnable()
    {
        if (interactable != null)
            interactable.selectFilters.Add(this);
    }

    private void OnDisable()
    {
        if (interactable != null)
            interactable.selectFilters.Remove(this);
    }

    /// <summary>
    /// Chamado pelo XRI pra validar se pode pegar o fio.
    /// true  = pode pegar
    /// false = bloqueia o grab
    /// </summary>
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactableObj)
    {
        // Com luva: libera, fim.
        if (PlayerHasGlove())
            return true;

        // Sem luva: só dá o feedback quando a pessoa REALMENTE tenta pegar (apertou o grip),
        // não só por estar mirando/encostando de leve.
        if (interactor.isSelectActive)
            GiveShockFeedback(interactor);

        // Bloqueia ou não, conforme o toggle.
        return !blockGrabWithoutGlove;
    }

    private bool PlayerHasGlove()
    {
        if (GloveManager.Instance == null)
        {
            Debug.LogWarning("[WireSafetyCheck] Sem GloveManager na cena — liberando o grab pra não travar o jogo.");
            return true; // fail-open de propósito: melhor liberar do que dar soft-lock
        }
        return GloveManager.Instance.HasGlove;
    }

    private void GiveShockFeedback(IXRSelectInteractor interactor)
    {
        if (Time.time - lastFeedbackTime < feedbackCooldown) return;
        lastFeedbackTime = Time.time;

        // 1) Vibração no controle que tentou pegar.
        FeedbackHelper.SendHaptic(interactor, hapticAmplitude, hapticDuration);

        // 2) Som de choque.
        if (shockSound != null)
        {
            if (audioSource != null) audioSource.PlayOneShot(shockSound);
            else AudioSource.PlayClipAtPoint(shockSound, transform.position);
        }

        // 3) Tremida de câmera.
        if (useCameraShake && CameraShake.Instance != null)
            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);

        // Extra: contador de acidentes, log, o que você quiser plugar.
        onUnsafeContact?.Invoke();
    }
}
