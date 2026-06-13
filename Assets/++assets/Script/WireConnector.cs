using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// =======================================================================
// Substitui o MovablePair + MoveWithMouseDrag.
// Vai na ponta MÓVEL do fio (a que o jogador agarra na mão).
//
// O XRGrabInteractable já faz a ponta seguir a mão sozinho — não precisa
// mais de OnMouseDrag nem ScreenToWorldPoint. Aqui só guardamos a
// identidade/cor e fazemos a ponta voltar pra origem quando solta no ar.
//
// Componentes necessários no mesmo GameObject:
//   - Rigidbody  (o XRGrabInteractable já exige; deixe "Use Gravity" DESLIGADO)
//   - Collider   (NÃO trigger)
//   - XRGrabInteractable
// =======================================================================
[RequireComponent(typeof(XRGrabInteractable))]
public class WireConnector : MonoBehaviour
{
    [Tooltip("Se a ponta for solta no ar (fora de um encaixe), ela volta pra posição de origem.")]
    public bool returnHomeWhenDropped = true;

    private XRGrabInteractable _grab;
    private Rigidbody _rb;
    private Vector3 _homePosition;
    private Quaternion _homeRotation;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        TryGetComponent(out _rb);

        _homePosition = transform.position;
        _homeRotation = transform.rotation;
    }

    private void OnEnable()  => _grab.selectExited.AddListener(OnReleased);
    private void OnDisable() => _grab.selectExited.RemoveListener(OnReleased);

    // Usado pelo MatchSystemManager pra embaralhar as posições iniciais.
    public Vector3 GetHomePosition() => _homePosition;

    public void SetHomePosition(Vector3 newHome)
    {
        _homePosition = newHome;
        transform.position = newHome;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (!returnHomeWhenDropped) return;

        // Se quem "soltou" foi um SOCKET (encaixe), ignora — não é o jogador
        // largando no ar, é o fio saindo de uma porta. Só reagimos quando a MÃO solta.
        if (args.interactorObject is XRSocketInteractor) return;

        // A mão soltou. Espera 1 frame pra ver se algum socket "pegou" a ponta no encaixe.
        StartCoroutine(ReturnHomeIfFree());
    }

    private IEnumerator ReturnHomeIfFree()
    {
        yield return null; // deixa os sockets processarem o encaixe neste frame

        // Se nada está selecionando a ponta (nenhum socket pegou), volta pra origem.
        if (!_grab.isSelected)
        {
            transform.SetPositionAndRotation(_homePosition, _homeRotation);

            if (_rb != null)
            {
                // zera a velocidade pra não "escorregar" depois de voltar.
                // OBS: em Unity mais antigo troque "linearVelocity" por "velocity".
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
