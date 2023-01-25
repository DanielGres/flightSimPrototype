using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    Renderer cubeRenderer;
    public float healthPoints = 20;

    public Color colorFine;
    public Color colorFucking;
    public Color colorFuucking;
    public Color colorFucked;

    float maxHLTH;
    void Start()
    {
        cubeRenderer = transform.GetComponent<Renderer>();
        maxHLTH = healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints > maxHLTH*2 / 3)
        {
            cubeRenderer.material.SetColor("_Color", Color.Lerp(colorFucking, colorFine, (healthPoints-2*(maxHLTH/3))/(maxHLTH/3)));
        }
        else if(healthPoints > maxHLTH / 3)
        {
            cubeRenderer.material.SetColor("_Color", Color.Lerp(colorFuucking, colorFucking, (healthPoints - (maxHLTH / 3)) / (maxHLTH/3)));
        }
        else
        {
            cubeRenderer.material.SetColor("_Color", Color.Lerp(colorFucked, colorFuucking, healthPoints / (maxHLTH / 3)));
        }
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            healthPoints -= 5f;
        }
    }
}
