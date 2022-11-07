using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public int heal = 100;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(heal <= 0)
            Destroy(gameObject);
    }

    IEnumerator TestCoroutine() {
        while(Vector3.Distance(transform.position, player.transform.position) > 0.01f) {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.2f * Time.deltaTime);
            yield return null;
        } 
    }

    public void LoseHeal(int dame) {
        heal -= dame;
    }
}
