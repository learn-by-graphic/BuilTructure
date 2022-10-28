using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public float time =100f;
    public float target;
    public float target_2;
    public Text time_text;
    public bool test_1;
    void Start()
    {
        if (test_1)
        {
            StartCoroutine("timer");
        }
        else
        {
            StartCoroutine("timer_action");
        }
        
    }


    IEnumerator timer()
    {
        yield return new WaitUntil(() => {
            if (time <= 0)
            {
                return true;
            }
            else
            {

                if (time <= target)
                {
                    if (dialog.instance.dialog_read(0) && !dialog.instance.running)
                    {
                        IEnumerator dialog_co = dialog.instance.dialog_system_start(0);
                        StartCoroutine(dialog_co);
                        
                        if (dialog.instance.dialog_read(0))
                        {
                            return false;
                        }

                    }else if (!dialog.instance.dialog_read(0) && !dialog.instance.running)
                    {
                        time -= Time.deltaTime;
                        time_text.text = time.ToString();
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                    time_text.text = time.ToString();
                }
                
                return false;
            }
        });
    }

    IEnumerator timer_action()
    {
        yield return new WaitUntil(() => {
            if (time <= 0)
            {
                return true;
            }
            else
            {

                if (time <= target && time>=target_2)
                {
                    if (dialog.instance.dialog_read(0) && !dialog.instance.running)
                    {
                        IEnumerator dialog_co = dialog.instance.dialog_system_start(0);
                        StartCoroutine(dialog_co);

                        if (dialog.instance.dialog_read(0))
                        {
                            return false;
                        }

                    }
                    else if (!dialog.instance.dialog_read(0) && !dialog.instance.running)
                    {
                        time -= Time.deltaTime;
                        time_text.text = time.ToString();
                    }

                }else if(time <= target_2)
                {
                    if (dialog.instance.dialog_read(1) && !dialog.instance.running)
                    {
                        IEnumerator dialog_co = dialog.instance.dialog_system_start(1);
                        StartCoroutine(dialog_co);

                        if (dialog.instance.dialog_read(1))
                        {
                            return false;
                        }

                    }
                    else if (!dialog.instance.dialog_read(1) && !dialog.instance.running)
                    {
                        time -= Time.deltaTime;
                        time_text.text = time.ToString();
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                    time_text.text = time.ToString();
                }

                return false;
            }
        });
    }
}
