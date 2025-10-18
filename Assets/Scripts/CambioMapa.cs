using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CambioMapa : MonoBehaviour
{
    public GameObject objectDissapear;
    public GameObject objectDissapear2;
    public GameObject objectAppear;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Movement>() != null)
        {

            Movement player=collision.gameObject.GetComponent<Movement>();

            objectDissapear.SetActive(false);
            objectDissapear2.SetActive(false);
            objectAppear.SetActive(true);

        }
        if (collision.gameObject.GetComponent<Movement1>() != null)
        {

            Movement1 player = collision.gameObject.GetComponent<Movement1>();

            objectDissapear.SetActive(false);
            objectDissapear2.SetActive(false);
            objectAppear.SetActive(true);

        }
        if (collision.gameObject.GetComponent<Movement2>() != null)
        {

            Movement2 player = collision.gameObject.GetComponent<Movement2>();

            objectDissapear.SetActive(false);
            objectDissapear2.SetActive(false);
            objectAppear.SetActive(true);


        }
    }

}
