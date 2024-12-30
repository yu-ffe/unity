using System;
using Unity.VisualScripting;
using UnityEngine;

public class Database : MonoBehaviour {

    public static Database Instance;
    public PlayerData playerData;

    public class PlayerData {
        public string[] Brids;

        public PlayerData() {
            Brids = new string[3];
            Brids[0] = "Bird1";
            Brids[1] = "Bird2";
            Brids[2] = "Bird3";
        }
    }

    void Awake() {
        Instance = this; // 싱글톤 할당
        playerData = new PlayerData();
    }
}
