using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject itemInside;
    public bool isItemPrefab = true;
    public GameObject effect;
    public int Health = 1;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            if (isItemPrefab) Instantiate(itemInside, transform.position, transform.rotation);
            else itemInside.transform.position = transform.position;
            GameObject effect_instance = Instantiate(effect, transform.position, transform.rotation);
            effect_instance.GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
        }

        if ( gameObject.GetComponent<QuestTrigger>() ) gameObject.GetComponent<QuestTrigger>().AdvanceQuest();
    }
}
