using UnityEngine;
using TMPro;

public class PlacaDias : MonoBehaviour
{
    [SerializeField] private TMP_Text display; // o número DESTA placa

    // ao adicionar o script, tenta achar o texto sozinho (confira se pegou o número, não o rótulo)
    void Reset() => display = GetComponentInChildren<TMP_Text>();

    void OnEnable()
    {
        if (ContadorDias.Instance != null) ContadorDias.Instance.Registrar(this);
    }

    void Start()
    {
        // segurança: se na OnEnable o contador ainda não existia, registra agora
        if (ContadorDias.Instance != null) ContadorDias.Instance.Registrar(this);
    }

    void OnDisable()
    {
        if (ContadorDias.Instance != null) ContadorDias.Instance.Desregistrar(this);
    }

    // o ContadorDias chama isto pra atualizar o número
    public void Mostrar(int dias)
    {
        if (display != null) display.text = dias.ToString();
        // pra ficar tipo "07", troque por: dias.ToString("00")
    }
}
