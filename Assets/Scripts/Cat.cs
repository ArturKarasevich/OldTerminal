using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject[] catSprites;
    public GameObject[] catSpritesToSleep;
    public GameObject[] catSpritesToActive;
    public GameObject[] catSpritesEar;
    public GameObject[] catSpritesBlink;
    public float catDelay = 0.1f;
    public string state = "";

    void Start()
    {
        state = "active";
        StartCoroutine(catAnim());
    }

    IEnumerator catAnim()
    {

        catSprites[0].SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (state == "active")
            {
                if (Random.Range(2, 7) == 6 )
                {
                    hideAll();
                    for (int i = 0; i < catSpritesBlink.Length; i++)
                    {
                        if (i > 0)
                            catSpritesBlink[i-1].SetActive(false);
                        catSpritesBlink[i].SetActive(true);
                        yield return new WaitForSeconds(catDelay);
                    }
                    yield return new WaitForSeconds(2);
                }
                else if (Random.Range(0, 20) == 6)
                {
                    hideAll();
                    for (int i = 0; i < catSpritesToSleep.Length; i++)
                    {
                        if (i > 0)
                            catSpritesToSleep[i - 1].SetActive(false);
                        catSpritesToSleep[i].SetActive(true);
                        yield return new WaitForSeconds(catDelay);
                    }
                    state = "sleep";
                    yield return new WaitForSeconds(3);
                }
            }
            else if (state == "sleep")
            {
                if (Random.Range(2, 7) == 6)
                {
                    hideAll();
                    for (int i = 0; i < catSpritesEar.Length; i++)
                    {
                        if (i > 0)
                            catSpritesEar[i - 1].SetActive(false);
                        catSpritesEar[i].SetActive(true);
                        yield return new WaitForSeconds(catDelay);
                    }
                    yield return new WaitForSeconds(5);
                }
                else if (Random.Range(0, 20) == 6)
                {
                    hideAll();
                    for (int i = 0; i < catSpritesToActive.Length; i++)
                    {
                        if (i > 0)
                            catSpritesToActive[i - 1].SetActive(false);
                        catSpritesToActive[i].SetActive(true);
                        yield return new WaitForSeconds(catDelay);
                    }
                    state = "active";
                    yield return new WaitForSeconds(3);
                }
            }
        }
    }

    void hideAll()
    {
        for (int i = 0;i < catSprites.Length;i++)
        {
            catSprites[i].SetActive(false);
        }
    }
}
