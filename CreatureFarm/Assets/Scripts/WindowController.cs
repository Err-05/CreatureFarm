using System;

using System.Runtime.InteropServices;

using UnityEngine;



public class WindowController : MonoBehaviour

{

    private IntPtr hWnd;



    // Windows API 

    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")] private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("Dwmapi.dll")] private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);



    private struct MARGINS { public int cxLeftWidth; public int cxRightWidth; public int cyTopHeight; public int cyBottomHeight; }



    // 윈도우 스타일 상수 

    private const int GWL_EXSTYLE = -20;

    private const int GWL_STYLE = -16;

    private const uint WS_EX_LAYERED = 0x00080000;

    private const uint WS_EX_TRANSPARENT = 0x00000020;

    private const uint WS_POPUP = 0x80000000; // 테두리 제거 핵심 

    private const uint WS_VISIBLE = 0x10000000;

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);



    void Start()

    {

#if !UNITY_EDITOR

        hWnd = GetActiveWindow(); 

 

        // 1. 테두리 없애기 (Popup 스타일 적용) 

        SetWindowLong(hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE); 

 

        // 2. 배경 투명하게 만들기 

        MARGINS margins = new MARGINS { cxLeftWidth = -1 }; 

        DwmExtendFrameIntoClientArea(hWnd, ref margins); 

 

        // 3. 창을 항상 위로 고정 

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0x0002 | 0x0001 | 0x0040); 

 

        // 4. 초기화 

        SetClickThrough(false); 

#endif

    }



    public void SetClickThrough(bool isThrough)

    {

#if !UNITY_EDITOR

        uint style = GetWindowLong(hWnd, GWL_EXSTYLE); 

        if (isThrough) SetWindowLong(hWnd, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT); 

        else SetWindowLong(hWnd, GWL_EXSTYLE, style & ~WS_EX_TRANSPARENT); 

#endif

    }

}