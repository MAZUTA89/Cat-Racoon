using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MultiplayersBuildAndRun
{
  [MenuItem("Multiplayer/Build and Run %#z")]
  static void BuildAndRun ()
  {
    PerformWin64Build(1);
    string dataPathRev = Reverse(Application.dataPath);
    string batPath = dataPathRev.Substring(dataPathRev.IndexOf("/"), dataPathRev.Length - dataPathRev.IndexOf("/"));
    batPath = Reverse(batPath) + "Builds/run.bat";
    Debug.Log(batPath);
    System.Diagnostics.Process.Start(batPath);
  }

  static void PerformWin64Build (int playerCount)
  {
    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

    for (int i = 1; i <= playerCount; i++)
    {
      BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + i.ToString() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.AllowDebugging);
    }
  }

  static string GetProjectName ()
  {
    string[] s = Application.dataPath.Split('/');
    return s[s.Length - 2];
  }

  static string[] GetScenePaths ()
  {
    string[] scenes = new string[EditorBuildSettings.scenes.Length];

    for (int i = 0; i < scenes.Length; i++)
    {
      scenes[i] = EditorBuildSettings.scenes[i].path;
    }

    return scenes;
  }

  public static string Reverse (string s)
  {
    char[] charArray = s.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
  }

}