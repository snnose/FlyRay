using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MaroControl : MonoBehaviour
{
    private Animator maroAnimator;

    // Start is called before the first frame update
    void Start()
    {
        maroAnimator = this.GetComponent<Animator>();
        maroAnimator.SetBool("MaroPush", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseControl.Instance.IsPause())
            this.transform.Translate(new Vector3(0.01f, (float)Mathf.Sin(Time.time) * Time.deltaTime, 0f));
    }

    public void StartPush()
    {
        maroAnimator.SetBool("MaroPush", true);
    }
}
