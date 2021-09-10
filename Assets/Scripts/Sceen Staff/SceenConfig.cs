using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceenConfig : MonoBehaviour
{ 

  public Transform mainGame;
  ScreenManag _mgr;

    bool pauseOn = false;


  private void Start()
  {
        _mgr = ScreenManag.Instance;

        _mgr.Push(new ScreenGO(mainGame));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

  private void Update()
  {

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && !pauseOn)
        {
            var s = Instantiate(Resources.Load<ScreenPause>("CanvasPause")); 
            _mgr.Push(s);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            pauseOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseOn)
        {
            _mgr.Pop();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseOn = false;

        }
    }

}
