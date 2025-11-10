using UnityEngine;

public class ParallaxMedellin : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] Background;
    Material[] mat;
    float[] backSpeed;
    Vector3[] bgStartPos; // Guardamos la posición inicial de cada plano

    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float ParallaxSpeed = 0.02f;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        Background = new GameObject[backCount];
        bgStartPos = new Vector3[backCount];

        // Obtener todos los planos y sus materiales, y guardar posición inicial
        for (int i = 0; i < backCount; i++)
        {
            Background[i] = transform.GetChild(i).gameObject;
            mat[i] = Background[i].GetComponent<Renderer>().material;
            bgStartPos[i] = Background[i].transform.position;
        }

        // Buscar el fondo más lejano (en eje Z)
        farthestBack = float.MinValue;
        foreach (var bg in Background)
        {
            if (bg.transform.position.z > farthestBack)
                farthestBack = bg.transform.position.z;
        }

        // Calcular las velocidades
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            // 1 = el más cercano, 0 = el más lejano
            backSpeed[i] = 1 - (Background[i].transform.position.z - cam.position.z) / (farthestBack - cam.position.z);
        }
    }
    public void FixedUpdate()
    {
        distance = camStartPos.x - cam.position.x;

        for (int i = 0; i < Background.Length; i++)
        {
            float speed = (backSpeed[i] + 0.2f) * ParallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(-distance * speed, 0));
        }
    }
}


