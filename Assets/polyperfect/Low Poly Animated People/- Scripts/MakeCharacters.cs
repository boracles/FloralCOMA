﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Polyperfect.Common;
//using UnityEditor.Formats.Fbx.Exporter;

namespace Polyperfect.People
{
    [ExecuteInEditMode]
    public class MakeCharacters : MonoBehaviour
    {
        public GameObject MainRig;

        public RuntimeAnimatorController[] randomRuntimeControllers;

        public string pathToCurrentFolder;
        public string pathToFBXFolder;

        [System.Serializable]
        public class CustomSetUp
        {
            public string meshName;

            public RuntimeAnimatorController runtimeController;

            public string wanderSpeciesName;

            public AIStats stats;
        }

        public CustomSetUp[] customSetUp;

        [ContextMenu("Make Rigs")]
        void MakeNewRigs()
        {
            //var components = GetComponents<Component>();

            for (int i = 0; i < MainRig.transform.Find("Geometry").childCount; i++)
            {
                var newRig = Instantiate(MainRig, transform);

                var id = UnityEngine.Random.Range(0, randomRuntimeControllers.Length);

                var controller = randomRuntimeControllers[id];

                for (int meshID = 0; meshID < MainRig.transform.Find("Geometry").childCount; meshID++)
                {
                    newRig.transform.Find("Geometry").GetChild(meshID).gameObject.SetActive(false);

                    if (meshID == i)
                    {
                        newRig.transform.Find("Geometry").GetChild(meshID).gameObject.SetActive(true);
                        newRig.transform.name = MainRig.transform.Find("Geometry").GetChild(meshID).gameObject.name;

                        for (int setUpIndex = 0; setUpIndex < customSetUp.Length; setUpIndex++)
                        {
                            if (newRig.name.Contains(customSetUp[setUpIndex].meshName))
                            {
                                controller = customSetUp[setUpIndex].runtimeController;
                                newRig.GetComponent<Common_WanderScript>().species = customSetUp[setUpIndex].wanderSpeciesName;
                                newRig.GetComponent<Common_WanderScript>().stats = customSetUp[setUpIndex].stats;
                            }
                        }
                    }
                }

                newRig.GetComponent<Animator>().runtimeAnimatorController = controller;
            }

        }

#if UNITY_EDITOR
        [ContextMenu("Make Prefabs")]
        void MakePrefabs()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Debug.Log(pathToCurrentFolder + "/" + transform.GetChild(i).transform.name + ".prefab");

                PrefabUtility.SaveAsPrefabAsset(transform.GetChild(i).gameObject, pathToCurrentFolder + "/" + transform.GetChild(i).transform.name + ".prefab");
            }
        }

        //Causing bugs in newer versions of unity
        /*

        [ContextMenu("Get Path")]
        void GetPath()
        {
            pathToCurrentFolder = EditorExtensions.GetPath();
        }
*/
#endif

//#if UNITY_EDITOR
//        [ContextMenu("Make FBX Rigs")]
//        void MakeFBXRigs()
//        {
//            for (int i = 0; i < transform.childCount; i++)
//            {
//                var path = pathToFBXFolder + "/" + transform.GetChild(i).transform.name + ".fbx";

//                var rends = transform.GetChild(i).transform.Find("Geometry").GetComponentsInChildren<SkinnedMeshRenderer>(true);

//                if (rends.Length == 0) return;

//                //Unity FBX Exporter requires this
//                foreach (var item in rends)
//                {
//                    if(!item.gameObject.activeSelf)
//                    item.enabled = false;
//                }

//                ModelExporter.ExportObject(path, transform.GetChild(i).gameObject);

//                //Unity FBX Exporter requires this
//                foreach (var item in rends)
//                {
//                    if (!item.gameObject.activeSelf)
//                        item.enabled = true;
//                }

//                //PrefabUtility.SaveAsPrefabAsset(transform.GetChild(i).gameObject, pathToCurrentFolder + "/" + transform.GetChild(i).transform.name + ".prefab");
//            }
//        }

//        //Causing bugs in newer versions of unity
//        /*

//        [ContextMenu("Get Path")]
//        void GetPath()
//        {
//            pathToCurrentFolder = EditorExtensions.GetPath();
//        }
//*/
//#endif

    }
}
