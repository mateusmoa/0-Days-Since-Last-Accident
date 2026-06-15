using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Coloque junto ao XR Grab Interactable do objeto LUVA (o que fica no chão/mesa).
/// Ao pegar: equipa a luva no GloveManager (liga o estado + pinta os controles de azul)
/// e some com o objeto.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class GloveEquip : MonoBehaviour
{
    [Header("Feedback opcional ao pegar a luva")]
    [SerializeField] private AudioClip pickupSound;
    [Tooltip("Vibração de confirmação ao equipar (0 a 1).")]
    [Range(0f, 1f)][SerializeField] private float hapticAmplitude = 0.3f;
    [SerializeField] private float hapticDuration = 0.1f;

    private XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (GloveManager.Instance == null)
        {
            Debug.LogWarning("[GloveEquip] Nenhum GloveManager na cena. Crie um GameObject vazio com o GloveManager.");
            return;
        }

        // 1) Liga o estado + pinta os dois controles de azul.
        GloveManager.Instance.EquipGlove();

        // 2) Feedbackzinho de confirmação (opcional).
        FeedbackHelper.SendHaptic(args.interactorObject, hapticAmplitude, hapticDuration);
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 3) Some com a luva do mundo.
        Destroy(gameObject);
    }
}
