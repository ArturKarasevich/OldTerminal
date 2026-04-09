using NUnit.Framework;
using UnityEngine;

public class CatEyes : MonoBehaviour
{
    public GameObject[] catPoses;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.mousePosition.x < 1000)
        {
            if (Input.mousePosition.y > 580)
            {
                catPoses[0].SetActive(true);
                catPoses[1].SetActive(false);
                catPoses[2].SetActive(false);
                catPoses[3].SetActive(false);
                catPoses[4].SetActive(false);
            }
            else if (Input.mousePosition.y > 460 && Input.mousePosition.y < 580)
            {
                catPoses[0].SetActive(false);
                catPoses[1].SetActive(true);
                catPoses[2].SetActive(false);
                catPoses[3].SetActive(false);
                catPoses[4].SetActive(false);
            }
            else if (Input.mousePosition.y > 340 && Input.mousePosition.y < 460)
            {
                catPoses[0].SetActive(false);
                catPoses[1].SetActive(false);
                catPoses[2].SetActive(true);
                catPoses[3].SetActive(false);
                catPoses[4].SetActive(false);
            }
            else if (Input.mousePosition.y > 220 && Input.mousePosition.y < 340)
            {
                catPoses[0].SetActive(false);
                catPoses[1].SetActive(false);
                catPoses[2].SetActive(false);
                catPoses[3].SetActive(true);
                catPoses[4].SetActive(false);
            }
            else if (Input.mousePosition.y > 220)
            {
                catPoses[0].SetActive(false);
                catPoses[1].SetActive(false);
                catPoses[2].SetActive(false);
                catPoses[3].SetActive(false);
                catPoses[4].SetActive(true);
            }
        }
    }
}
