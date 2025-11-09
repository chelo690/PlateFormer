using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausarJuego : MonoBehaviour
{

    public GameObject menuPausa;
    public bool JuegoPausado = false;

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
                
            }
            else
            {
                
                Pausar();
            }
        }

    }
    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1;
        JuegoPausado = false;

    }
    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0;
        JuegoPausado = true;
    }
    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
