using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public GameObject goCube;
    public GameObject goQuad;
    public GameObject goExplosion;

    private Coroutine crExploding;

    void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            if (crExploding != null)
                StopCoroutine(crExploding);

            crExploding = StartCoroutine(_Exploding());
            GameController.instance.IncrementScore();
        }
    }

    IEnumerator _Exploding ()
    {
        goCube.SetActive(false);
        goQuad.SetActive(false);
        goExplosion.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        goCube.SetActive(true);
        goQuad.SetActive(true);
        goExplosion.SetActive(false);
    }
}
