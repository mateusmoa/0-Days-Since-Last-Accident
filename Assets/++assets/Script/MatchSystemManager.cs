using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// Evento que carrega (acertos, total) pra mostrar progresso tipo "3/6".
// Precisa dessa subclasse pra um UnityEvent genérico aparecer no Inspector.
[System.Serializable]
public class ProgressEvent : UnityEvent<int, int> { }

// =======================================================================
// O "cérebro": sorteia cores, embaralha posições e conta os acertos.
// Dispara UnityEvents pra você reagir no Inspector quando a tarefa termina
// (onAllConnected) ou a cada conexão (onProgressChanged).
// =======================================================================
public class MatchSystemManager : MonoBehaviour
{
    public List<Material> colorMaterials;

    [Tooltip("Se ligado, embaralha as posições de origem das pontas.")]
    public bool randomizeStartPositions = true;

    [Header("Reações configuráveis no Inspector")]
    public UnityEvent onAllConnected;        // TUDO certo! (todos os fios conectados)
    public ProgressEvent onProgressChanged;  // (acertos, total) a cada mudança

    private List<MatchEntity> _matchEntities;
    private int _targetMatchCount;
    private int _currentMatchCount = 0;

    private void Start()
    {
        _matchEntities = GetComponentsInChildren<MatchEntity>().ToList();
        _targetMatchCount = _matchEntities.Count;

        SetEntityColors();
        if (randomizeStartPositions) RandomizeConnectorPlacement();

        onProgressChanged.Invoke(_currentMatchCount, _targetMatchCount); // estado inicial 0/N
    }

    private void SetEntityColors()
    {
        if (colorMaterials.Count < _matchEntities.Count)
        {
            Debug.LogError($"Faltam cores: {_matchEntities.Count} cabos, " +
                           $"mas só {colorMaterials.Count} materiais na lista.", this);
            return;
        }

        Shuffle(colorMaterials);
        for (int i = 0; i < _matchEntities.Count; i++)
            _matchEntities[i].SetColor(colorMaterials[i]);
    }

    private void RandomizeConnectorPlacement()
    {
        var homes = new List<Vector3>();
        for (int i = 0; i < _matchEntities.Count; i++)
            homes.Add(_matchEntities[i].GetConnectorHome());

        Shuffle(homes);
        for (int i = 0; i < _matchEntities.Count; i++)
            _matchEntities[i].SetConnectorHome(homes[i]);
    }

    public void NewMatchRecord(bool matchConnected)
    {
        _currentMatchCount += matchConnected ? 1 : -1;
        Debug.Log($"Conexões corretas: {_currentMatchCount}/{_targetMatchCount}");

        onProgressChanged.Invoke(_currentMatchCount, _targetMatchCount);

        if (_currentMatchCount == _targetMatchCount)
        {
            Debug.Log("MANDOU BEM! TODOS OS FIOS CONECTADOS!");
            onAllConnected.Invoke();
        }
    }

    // Fisher-Yates.
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
