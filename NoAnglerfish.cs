using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UIElements;

public class RemoveAnglerfish : Mod
{
    #region Variables

    static bool removeAnglerfish;

    #endregion

    public void Start()
    {
        Debug.Log("Mod RemoveAnglerfish has been loaded!");

        var harmony = new Harmony("com.KUNGERMOoN.RemoveAnglerfish");
        harmony.PatchAll();
    }

    public void OnModUnload()
    {
        Debug.Log("Mod RemoveAnglerfish has been unloaded!");
    }

    #region Patches

    [HarmonyPatch(typeof(AI_NetworkBehaviour), "Update")]
    class AnimalsPatch
    {
        static void Prefix(AI_NetworkBehaviour __instance)
        {
            if (!(__instance is AI_NetworkBehaviour_Domestic) & !(__instance is AI_NetworkBehaviour_BugSwarm))
            {
                if (__instance is AI_NetworkBehaviour_AnglerFish && removeAnglerfish)
                    Destroy(__instance.gameObject);
            }
        }
    }

    #endregion

    #region Settings
    //I used Extra Settings API (https://www.raftmodding.com/mods/extra-settings-api) for custom mod settings
    static HarmonyLib.Traverse ExtraSettingsAPI_Traverse;
    static bool ExtraSettingsAPI_Loaded = false;

    public void ExtraSettingsAPI_Load() // Occurs when the API loads the mod's settings
    {
        LoadSettings();
    }

    public void ExtraSettingsAPI_SettingsClose() // Occurs when user closes the settings menu
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        removeAnglerfish = ExtraSettingsAPI_GetCheckboxState("removeAnglerfish");
    }

    public bool ExtraSettingsAPI_GetCheckboxState(string SettingName)
    {
        if (ExtraSettingsAPI_Loaded)
            return ExtraSettingsAPI_Traverse.Method("getCheckboxState", new object[] { this, SettingName }).GetValue<bool>();
        return false;
    }

    #endregion
}