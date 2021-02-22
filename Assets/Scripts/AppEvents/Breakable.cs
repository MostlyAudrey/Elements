using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject itemInside;
    public GameObject effect;
    public int Health = 1;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Instantiate(itemInside, transform.position, transform.rotation);
            GameObject effect_instance = Instantiate(effect, transform.position, transform.rotation);
            effect_instance.GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
        }
    }
}
