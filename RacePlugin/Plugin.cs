using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.Networking;

namespace RacePlugin
{
    public class Plugin : IPlugin
    {

        public GameObject raceTrack;
        UnityWebRequest www;

        public bool loaded = false;
        public bool shouldload = false;
        int currentLevelId = 0;

        public string Name
        {
            get { return "Race Plugin"; }
        }
        public string Version
        {
            get { return "0.1"; }
        }

        public void OnApplicationQuit()
        {
        }

        public void OnApplicationStart()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void spawnLocalTrack()
        {
            if (raceTrack)
            {
                UnityEngine.GameObject.DestroyImmediate(raceTrack);
            }

            if(!raceTrack)
            {
                AssetBundle ab = AssetBundle.LoadFromFile("D:\\letsrace");
                GameObject prefab = ab.LoadAsset<GameObject>("LetsRace");
                raceTrack = UnityEngine.Object.Instantiate(prefab);
                raceTrack.transform.position = new Vector3(0f,0f,0f);
                ab.Unload(false);
                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Spikes");
                foreach(GameObject spike in objects)
                {
                    spike.tag = "Spikes";
                }
            }
        }

        public void spawnRemoteTrack()
        {
            if (raceTrack)
            {
                UnityEngine.GameObject.DestroyImmediate(raceTrack);
                loaded = false;
            }

            if (!raceTrack)
            {
                www = UnityWebRequest.GetAssetBundle("https://mods.jet-is.land/test/letsrace");
                www.Send();
                shouldload = true;
                loaded = false;
            }
        }

        public void checkDownload()
        {
            if(www != null)
            {
                if(www.isDone)
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(www);
                    GameObject prefab = ab.LoadAsset<GameObject>("LetsRace");
                    raceTrack = UnityEngine.Object.Instantiate(prefab);
                    raceTrack.transform.position = new Vector3(0f, 0f, 0f);
                    ab.Unload(false);
                    var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Spikes");
                    foreach (GameObject spike in objects)
                    {
                        spike.tag = "Spikes";
                    }
                    shouldload = false;
                    loaded = true;
                }
            }
        }

        public void OnLevelWasInitialized(int level)
        {
            currentLevelId = level;
            if (currentLevelId == 1)
            {
                spawnRemoteTrack();
            }
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
            if (currentLevelId == 1)
            {

                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    spawnLocalTrack();
                }

                if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    spawnRemoteTrack();
                }

                if (loaded == false && shouldload == true)
                    checkDownload();

                if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PlayerBody.localPlayer.movement.currentMovement = Vector3.zero;
                    PlayerBody.localPlayer.transform.root.position = new Vector3(-4567.9f, 107.8f, 9696.2f);
                }
                /*
                if(PlayerBody.localPlayer.input.leftTriggerPulled)
                {
                    Console.WriteLine(PlayerBody.localPlayer.body.torsoParent.position.ToString());        
                }
                */
            }
        }
    }
}
