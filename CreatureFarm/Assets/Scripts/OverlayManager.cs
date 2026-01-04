using UnityEngine;

using UnityEngine.EventSystems;



public class OverlayManager : MonoBehaviour

{

    public WindowController windowController;

    public LayerMask hitLayer; // Inspector에서 'Character' 레이어 체크 필수 

    private bool isPassThroughMode = false;



    void Update()

    {

        bool isHitObject = false;



        // 1. 캐릭터/바닥 감지 (Raycast) 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, 100f, hitLayer)) isHitObject = true;



        // 2. UI 감지 (EventSystem) 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) isHitObject = true;



        // 상태 전환 로직 

        bool nextMode = !isHitObject;

        if (isPassThroughMode != nextMode)

        {

            isPassThroughMode = nextMode;

            windowController.SetClickThrough(nextMode);

        }

    }

}