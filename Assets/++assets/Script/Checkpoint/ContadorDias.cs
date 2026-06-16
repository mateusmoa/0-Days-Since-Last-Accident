using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContadorDias : MonoBehaviour
{
    public static ContadorDias Instance { get; private set; }

    [SerializeField] private TMP_Text display;     // o número na placa
    [SerializeField] private int diasIniciais = 0;

    private int dias;
    private readonly HashSet<int> visitados = new(); // ids dos checkpoints já batidos

    void Awake()
    {
        Instance = this;
        dias = diasIniciais;
    }

    void Start() => AtualizarDisplay();

    // Chamado ao bater um checkpoint. Só conta na PRIMEIRA vez que aquele ponto é batido.
    public void PassarFase(Component checkpoint, int incremento = 1)
    {
        if (checkpoint != null)
        {
            int id = checkpoint.GetInstanceID();
            if (visitados.Contains(id)) return; // já passou por aqui, não conta de novo
            visitados.Add(id);
        }

        dias += incremento;
        AtualizarDisplay();
    }

    // Chamado ao sofrer um acidente (morrer). Zera o contador — a piada do jogo.
    public void RegistrarAcidente()
    {
        dias = 0;
        AtualizarDisplay();
        // NÃO limpa 'visitados': ao voltar e re-bater o mesmo ponto, ele não soma de novo.
    }

    private void AtualizarDisplay()
    {
        if (display != null) display.text = dias.ToString();
        // pra ficar tipo "00", troque por: dias.ToString("00")
    }

    public int Dias => dias;
}
