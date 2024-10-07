using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject buff, debuff;
    private float buffTimeCounter;
    private float debuffTimeCounter;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            buffTimeCounter += Time.deltaTime;
            if (buffTimeCounter >= 1.2f)
            { 
                buffTimeCounter = 0f;
                buff.SetActive(false);
            }
        }

        if (debuff.activeInHierarchy)
        {
            debuffTimeCounter += Time.deltaTime;
            if (debuffTimeCounter >= 1.2f)
            {
                debuffTimeCounter = 0f;
                debuff.SetActive(false);
            }
        }
    }
}
