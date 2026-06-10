using Unity.XR.CoreUtils.Datums;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuExibir : MonoBehaviour
{
    public Transform jogador;
    public float distance = 3.0f;
    public GameObject menu;
    public InputActionProperty exibirBotao;

   void Update()
    {
        if (exibirBotao.action.WasPressedThisFrame())
        {
            menu.SetActive(value: !menu.activeSelf);
            menu.transform.position = jogador.position + new Vector3(x: jogador.forward.x, y: 0, z: jogador.forward.z).normalized;
        }

     menu.transform.LookAt(worldPosition:new Vector3(x: jogador.position.x, y: menu.transform.position.y,z: jogador.position.z));
        menu.transform.forward *= -1;
    }


}

