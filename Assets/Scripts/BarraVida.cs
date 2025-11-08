using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private Movement playerController;
    private float vidaMaxima;
    // Start is called before the first frame update
    void Start()
    {
       playerController=GameObject.Find("Jose Maria").GetComponent<Movement>();
        vidaMaxima = playerController.Health;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = playerController.Health / vidaMaxima;
    }
}
