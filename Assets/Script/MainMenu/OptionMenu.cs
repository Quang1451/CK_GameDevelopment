using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangAnimation(int valueChange){
        if(valueChange == 0){
            animator.CrossFade("Option", 0.5f,0);
        }
        if(valueChange == 1){
            animator.CrossFade("SoundButtonClick", 0.5f,0);
        }
        if(valueChange == 2){
            animator.CrossFade("GraphicsButtonClick", 0.5f,0);
        }
        if(valueChange == 3){
            animator.CrossFade("GamePlayButtonClick", 0.5f,0);
        }
        if(valueChange == 4){
            animator.CrossFade("LanguageButtonClick", 0.5f,0);
        }
    }

    public void OnExitButtonClick(){
        animator.SetTrigger("ExitButtonClick");
    }

    public void DisbleThisGameObject(){
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
