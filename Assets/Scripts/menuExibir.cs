using UnityEngine;
using UnityEngine.InputSystem;

public class MenuExibir : MonoBehaviour
{
    public Transform jogador;
    public float distance = 3f;
    public GameObject menu;
    public InputActionProperty exibirBotao;

    private void OnEnable()
    {
        exibirBotao.action.Enable();
    }

    private void OnDisable()
    {
        exibirBotao.action.Disable();
    }

    void Update()
    {
        if (exibirBotao.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);

            menu.transform.position =
                jogador.position +
                new Vector3(jogador.forward.x, 0, jogador.forward.z).normalized * distance;
        }

        if (menu.activeSelf)
        {
            menu.transform.LookAt(
                new Vector3(
                    jogador.position.x,
                    menu.transform.position.y,
                    jogador.position.z));

            menu.transform.forward *= -1;
        }
    }
}