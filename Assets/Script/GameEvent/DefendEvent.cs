using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class DefendEvent : MonoBehaviour
{
    [SerializeField] private Transform[] PointSpawnZombies;
    [SerializeField] private GameObject zb;

    [SerializeField] private TextMeshProUGUI showText;
    [SerializeField] private float timePrepare = 0;
    [SerializeField] private float timeDefend = 300;
    [SerializeField] private int amountMaxZombies;
    private string content;

    private float _TimePrepare;
    private bool _isStartEvent = false;
    // Start is called before the first frame update
    void Start()
    {
        showText.gameObject.SetActive(true);
        _TimePrepare = Time.time + timePrepare;
        content = "Ready for zombie attack, you have 30 seconds to prepare";
    }

    // Update is called once per frame
    void Update()
    {
        if(timePrepare > 0) {
            timePrepare -= Time.deltaTime;
            content = "Ready for zombie attack, you have "+(int)timePrepare+" seconds to prepare";
            showText.text = content;
        }
        else {
            showText.text = (int) (timeDefend/60) +":" + ((int) (timeDefend% 60)).ToString("00");
            timeDefend -= Time.deltaTime;
            if(!_isStartEvent) {
                InvokeRepeating("SpawnZB",2,3);
                _isStartEvent = true;
            }
        }

        if(timeDefend < 0) {
            SceneManager.LoadScene(5);
        }
    }

    void SpawnZB(){
        if(GameObject.FindGameObjectsWithTag("Enemy").Length < amountMaxZombies) {
            GameObject zombie1 = Instantiate(zb, PointSpawnZombies[Random.Range(0,PointSpawnZombies.Length -1)].position, Quaternion.identity);
            zombie1.GetComponent<EnemyAI>().SetChasing();
            GameObject zombie2 = Instantiate(zb, PointSpawnZombies[Random.Range(0,PointSpawnZombies.Length -1)].position, Quaternion.identity);
            zombie2.GetComponent<EnemyAI>().SetChasing();
            GameObject zombie3 = Instantiate(zb, PointSpawnZombies[Random.Range(0,PointSpawnZombies.Length -1)].position, Quaternion.identity);
            zombie3.GetComponent<EnemyAI>().SetChasing();
            GameObject zombie4 = Instantiate(zb, PointSpawnZombies[Random.Range(0,PointSpawnZombies.Length -1)].position, Quaternion.identity);
            zombie4.GetComponent<EnemyAI>().SetChasing();
        }
    }
}
