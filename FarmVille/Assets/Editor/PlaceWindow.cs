using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;

public class PlaceWindow : MonoBehaviour
{
  public TextMeshProUGUI outputText;

#if UNITY_STANDALONE_WIN
  [DllImport("user32.dll", EntryPoint = "SetWindowText")]
  public static extern bool SetWindowText (System.IntPtr hwnd, System.String lpString);
  
  [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
  private static extern bool SetWindowPos (IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
  [DllImport("user32.dll", EntryPoint = "FindWindow")]
  public static extern IntPtr FindWindow (System.String className, System.String windowName);

  public static void SetPosition (string name, int x, int y, int resX = 0, int resY = 0)
  {
    System.IntPtr windowPtr = FindWindow(null, "GAME WINDOW NAME");
    SetWindowText(windowPtr, name);
    SetWindowPos(FindWindow(null, name), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
  }

  public void PositionWindow(string name, int x, int y)
  {
    SetPosition(name, x, y);
  }

#endif
}