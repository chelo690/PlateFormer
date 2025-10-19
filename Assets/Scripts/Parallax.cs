using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Efecto Parallax")]
    [Tooltip("Movimiento en X (horizontal). 0 = fijo, 1 = igual a la cámara.")]
    public float parallaxMultiplierX = 0.5f;
    [Tooltip("Movimiento en Y (vertical). 0 = fijo, 1 = igual a la cámara.")]
    public float parallaxMultiplierY = 0f; // 0 por defecto para mantener el suelo estático

    [Header("Repetición infinita")]
    [Tooltip("Actívalo si quieres que el fondo se repita infinito horizontalmente")]
    public bool infiniteHorizontal = true;
    [Tooltip("Actívalo si quieres que el fondo se repita infinito verticalmente (opcional)")]
    public bool infiniteVertical = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        // Tamaño del sprite en unidades del mundo
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        // Aplicar movimiento del parallax
        transform.position += new Vector3(cameraDelta.x * parallaxMultiplierX, cameraDelta.y * parallaxMultiplierY, 0f);
        lastCameraPosition = cameraTransform.position;

        // Repetición infinita horizontal
        if (infiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetX, transform.position.y, transform.position.z);
            }
        }

        // Repetición infinita vertical
        if (infiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetY, transform.position.z);
            }
        }
    }
}
