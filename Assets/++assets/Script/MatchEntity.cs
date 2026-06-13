using UnityEngine;

// =======================================================================
// A "ponte" que liga a ponta (connector), a cor do encaixe, o feedback e o
// manager. Agora também guarda o PivotStart do fio e o move JUNTO com a ponta
// quando o manager embaralha as posições — assim o fio não cruza a cena.
// =======================================================================
public class MatchEntity : MonoBehaviour
{
    public MatchFeedback feedback;
    public WireConnector connector;
    public Renderer socketColorRenderer;
    public MatchSystemManager matchSystemManager;

    [Tooltip("O PivotStart deste fio (o MESMO objeto que está no campo 'start' do WireStretch). " +
             "Se preenchido, ele se move junto com a ponta no embaralhamento, pro fio não cruzar.")]
    public Transform startAnchor;

    private bool _matched;

    public Vector3 GetConnectorHome() => connector.GetHomePosition();

    public void SetConnectorHome(Vector3 newHome)
    {
        // Move a âncora (PivotStart) junto com a ponta, mantendo a distância entre as duas.
        // Assim o fio nasce curtinho no lugar novo, em vez de atravessar o cenário.
        if (startAnchor != null)
        {
            Vector3 offset = startAnchor.position - connector.GetHomePosition();
            startAnchor.position = newHome + offset;
        }

        connector.SetHomePosition(newHome);
    }

    public void SetColor(Material pairMaterial)
    {
        connector.GetComponent<Renderer>().material = pairMaterial;
        socketColorRenderer.material = pairMaterial;
    }

    // Chamado pelo WireSocket quando ALGUMA ponta entra/sai do encaixe.
    public void OnSocketInteraction(bool entered, WireConnector other)
    {
        if (entered && !_matched)
        {
            _matched = (other == connector); // só conta se for a ponta CERTA

            if (_matched)
            {
                matchSystemManager.NewMatchRecord(true);
                feedback.ChangeMaterialWithMatch(true);
            }
        }
        else if (!entered && _matched)
        {
            _matched = !(other == connector);

            if (!_matched)
            {
                matchSystemManager.NewMatchRecord(false);
                feedback.ChangeMaterialWithMatch(false);
            }
        }
    }
}
