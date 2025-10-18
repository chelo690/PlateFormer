using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleccionables2 : MonoBehaviour
{
    public GameObject objectDissapear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            Movement player = collision.gameObject.GetComponent<Movement>();
            player.playerHealth = - 10f; 
            Desaparecer();
        }

    }
    private void Desaparecer()
    {
        if (objectDissapear != null)
        {
            objectDissapear.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
