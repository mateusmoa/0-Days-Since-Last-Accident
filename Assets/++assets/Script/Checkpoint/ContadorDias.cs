using System.Collections.Generic;
using UnityEngine;

public class ContadorDias : MonoBehaviour
{
    public static ContadorDias Instance { get; private set; }

    [SerializeField] private int diasIniciais = 0;

    private int dias;
    private readonly HashSet<int> visitados = new();   // ids dos checkpoints já batidos
    private readonly List<PlacaDias> placas = new();    // todas as placas se registram aqui sozinhas

    void Awake()
    {
        Instance = this;
        dias = diasIniciais;
    }

    // chamado por cada placa quando ela aparece (ela mesma se anuncia)
    public void Registrar(PlacaDias placa)
    {
        if (!placas.Contains(placa)) placas.Add(placa);
        placa.Mostrar(dias); // já mostra o número certo na placa recém-chegada
    }

    public void Desregistrar(PlacaDias placa) => placas.Remove(placa);

    // chamado ao bater um checkpoint. Só conta na PRIMEIRA vez que aquele ponto é batido.
    public void PassarFase(Component checkpoint, int incremento = 1)
    {
        if (checkpoint != null)
        {
            int id = checkpoint.GetInstanceID();
            if (visitados.Contains(id)) return; // já passou por aqui, não conta de novo
            visitados.Add(id);
        }

        dias += incremento;
        AtualizarTodas();
    }

    // chamado ao sofrer um acidente (morrer). Zera o contador — a piada do jogo.
    public void RegistrarAcidente()
    {
        dias = 0;
        AtualizarTodas();
        // NÃO limpa 'visitados': ao voltar e re-bater o mesmo ponto, ele não soma de novo.
    }

    private void AtualizarTodas()
    {
        foreach (var p in placas)
            if (p != null) p.Mostrar(dias);
    }

    public int Dias => dias;
}
