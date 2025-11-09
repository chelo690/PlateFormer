using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceNat : MonoBehaviour
{

    [SerializeField] private Animator NatillaInt;

    [SerializeField] private float bounceDuration = 0.3f;

    private void Start()
    {
        NatillaInt = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto pertenece al grupo "Jugadores"
        if (other.transform.parent != null && other.transform.parent.name == "Jugadores")
        {
            // Verifica nombres específicos por seguridad
            if (other.name == "Jose Maria" || other.name == "Maria Jose")
            {
                // Cambia el bool para activar la animación
                NatillaInt.SetBool("isStep", true);

                // Inicia una corrutina para devolverlo a false
                StartCoroutine(ResetBounce());
            }
        }
    }

    private System.Collections.IEnumerator ResetBounce()
    {
        yield return new WaitForSeconds(bounceDuration);
        NatillaInt.SetBool("isStep", false);
    }
}
