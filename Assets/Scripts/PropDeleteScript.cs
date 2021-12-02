using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDeleteScript : MonoBehaviour
{

    public ParticleSystem Animation;
    public AudioClip DestroySound;
    private bool isCollected = false;

    void Start()
    {
        GameManager.Instance.AllItems++;
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&&isCollected==false)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Animation.transform.position = transform.position;
        Animation.Play();
        AudioSource.PlayClipAtPoint(DestroySound, transform.position);
        Destroy(gameObject, 0.25f);
        GameManager.Instance.DestroyedItems++;
        isCollected = true;
    }
}
