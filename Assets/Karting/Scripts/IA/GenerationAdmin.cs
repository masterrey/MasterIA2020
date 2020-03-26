using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationAdmin : MonoBehaviour
{
    public GameObject iaprefab;
    public int karts=10;
    public IAInput[] iaKarts;
    float[] bestiaturnDNA;
    void Start()
    {
        iaKarts = new IAInput[karts];
        for (int i = 0; i < karts; i++)
        {
            GameObject kart=
            Instantiate(iaprefab, transform.position, transform.rotation);
            iaKarts[i] = kart.GetComponent<IAInput>();
            if (bestiaturnDNA != null)
            {
                iaKarts[i].SetDNA(bestiaturnDNA);
            }
            else
            {
                iaKarts[i].CreateDNA();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
