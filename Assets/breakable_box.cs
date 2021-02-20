using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable_box : MonoBehaviour
{
    public GameObject ItemInside;
    public int Health = 1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I'm here! ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
           Instantiate(ItemInside, transform.position, transform.rotation);
           gameObject.SetActive(false);
        }
    }
}
