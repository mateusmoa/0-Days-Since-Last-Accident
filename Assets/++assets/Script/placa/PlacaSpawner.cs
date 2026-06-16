using System.Collections.Generic;
using UnityEngine;

public class PlacaSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PlacaEtapa
    {
        public GameObject prefab;     // a placa desta etapa (mesh/textura própria)
        public Transform spawnPoint;  // onde ela aparece
    }

    [SerializeField] private List<PlacaEtapa> etapas = new();

    [Tooltip("Ligado: as placas vão se acumulando pelo cenário. Desligado: cada placa nova troca a anterior.")]
    [SerializeField] private bool acumular = true;

    private int index = 0;
    private GameObject ultimaPlaca;

    // Avança pra próxima placa da sequência. Chame ao MORRER.
    public GameObject SpawnNext()
    {
        if (etapas.Count == 0) return null;

        // no modo acumular, depois que todas foram colocadas não spawna mais nada
        if (acumular && index >= etapas.Count) return ultimaPlaca;

        int i = acumular ? index : (index % etapas.Count);
        PlacaEtapa etapa = etapas[i];

        if (etapa.prefab == null || etapa.spawnPoint == null)
        {
            index++;
            return null; // etapa mal configurada, pula sem quebrar
        }

        // no modo "trocar", remove a placa anterior antes de criar a nova
        if (!acumular && ultimaPlaca != null) Destroy(ultimaPlaca);

        ultimaPlaca = Instantiate(etapa.prefab, etapa.spawnPoint.position, etapa.spawnPoint.rotation);
        index++;
        return ultimaPlaca;
    }
}
