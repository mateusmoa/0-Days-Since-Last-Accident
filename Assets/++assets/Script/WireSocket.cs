using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// =======================================================================
// Substitui o Port + OnTriggerEnter/OnTriggerExit.
// Vai no ENCAIXE (a "porta" colorida do lado de destino).
//
// O XRSocketInteractor já faz tudo que o Port fazia, e melhor:
//   - detecta quando uma ponta chega perto;
//   - "puxa" e alinha a ponta no encaixe automaticamente;
//   - dispara selectEntered (encaixou) e selectExited (tirou).
//
// Aqui a gente só escuta esses eventos e avisa a MatchEntity, igualzinho
// o Port chamava _ownerMatchEntity.PairObjectInteraction(...).
//
// Componentes necessários no mesmo GameObject:
//   - XRSocketInteractor
//   - Collider marcado como "Is Trigger" (define a zona de encaixe)
// =======================================================================
[RequireComponent(typeof(XRSocketInteractor))]
public class WireSocket : MonoBehaviour
{
    [Tooltip("A MatchEntity dona deste encaixe.")]
    public MatchEntity owner;

    private XRSocketInteractor _socket;

    private void Awake() => _socket = GetComponent<XRSocketInteractor>();

    private void OnEnable()
    {
        _socket.selectEntered.AddListener(OnSocketEntered);
        _socket.selectExited.AddListener(OnSocketExited);
    }

    private void OnDisable()
    {
        _socket.selectEntered.RemoveListener(OnSocketEntered);
        _socket.selectExited.RemoveListener(OnSocketExited);
    }

    // Uma ponta encaixou aqui.
    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out WireConnector connector))
            owner.OnSocketInteraction(entered: true, connector);
    }

    // Uma ponta saiu daqui.
    private void OnSocketExited(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out WireConnector connector))
            owner.OnSocketInteraction(entered: false, connector);
    }
}
