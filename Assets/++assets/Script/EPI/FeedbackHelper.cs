using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Funçõezinhas auxiliares de feedback. Por enquanto só a vibração do controle,
/// mas serve de "lugar central" caso queira adicionar mais coisa depois.
/// </summary>
public static class FeedbackHelper
{
    /// <summary>
    /// Manda vibração pro controle que disparou o evento (o interactor que está segurando/tentando).
    /// Procura o HapticImpulsePlayer na hierarquia do controle automaticamente,
    /// então funciona pra qualquer uma das duas mãos sem precisar arrastar referência.
    /// </summary>
    public static void SendHaptic(IXRSelectInteractor interactor, float amplitude, float duration)
    {
        if (interactor == null) return;

        var haptics = interactor.transform.GetComponentInParent<HapticImpulsePlayer>();
        if (haptics != null)
            haptics.SendHapticImpulse(Mathf.Clamp01(amplitude), duration);
        else
            Debug.LogWarning("[FeedbackHelper] HapticImpulsePlayer não encontrado no controle — vibração ignorada. " +
                             "No rig padrão do XRI ele fica no GameObject do controle (Left/Right Controller).");
    }
}
