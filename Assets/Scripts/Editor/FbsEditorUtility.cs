/*
 * Utility script to manager editor stuffs like 
 * 1 - Clear PlayerPref
 * 2 - Clear Cache etc...
 */

using UnityEngine;
using UnityEditor;

public class FbsEditorUtility : Editor
{
    //Clear all local db PlayerPrefs
    [MenuItem("FBS/Clear PlayerPrefs")]
    public static void ClearAllLocalDB()
    {
        Debug.Log("Deletes all player prefabs");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("FBS/Clear AssetsBundle")]
    public static void ClearAssetsBundleCache()
    {
        if (Caching.ClearCache()) 
        {
            Debug.Log("Successfully cleaned the cache");
        }
        else 
        {
            Debug.Log("Cache is being used");
        }
    }
}