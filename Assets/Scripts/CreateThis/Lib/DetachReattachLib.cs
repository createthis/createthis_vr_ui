using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DetachReattachLib {
    private static Dictionary<GameObject, int> gameObjectDetachCount;
    private static bool hasInitialized;

    private static void Initialize() {
        if (hasInitialized) return;
        gameObjectDetachCount = new Dictionary<GameObject, int>();
        hasInitialized = true;
    }

    public static void IncrementDetachCount(GameObject gameObject) {
        Initialize();
        if (gameObjectDetachCount.ContainsKey(gameObject)) {
            gameObjectDetachCount[gameObject]++;
        } else {
            gameObjectDetachCount[gameObject] = 1;
        }
    }

    public static bool DetachInProgress(GameObject gameObject) {
        Initialize();
        if (gameObjectDetachCount.ContainsKey(gameObject)) return true;
        return false;
    }

    public static void DecrementDetachCount(GameObject gameObject) {
        Initialize();
        if (gameObjectDetachCount.ContainsKey(gameObject)) {
            if (gameObjectDetachCount[gameObject] == 1) gameObjectDetachCount.Remove(gameObject);
            else gameObjectDetachCount[gameObject]--;
        } else {
            Debug.Log("DecrementDetach less than zero for gameObject.name=" + gameObject.name); // this should never happen
        }
    }


    public static Transform[] GetChildren(GameObject gameObject) {
        Transform[] children = new Transform[gameObject.transform.childCount];
        int i = 0;
        foreach (Transform T in gameObject.transform) {
            children[i++] = T;
        }
        return children;
    }

    public static bool ChildrenEqual(GameObject gameObject, Transform[] compareChildren) {
        Transform[] children = GetChildren(gameObject);
        if (children.Length != compareChildren.Length) {
            Debug.Log("ChildrenEqual children.Length=" + children.Length + ",compareChildren.Length=" + compareChildren.Length);
            return false;
        }
        for (int i = 0; i < children.Length; i++) {
            Transform child = children[i];
            Transform compareChild = compareChildren[i];
            if (child.GetInstanceID() != compareChild.GetInstanceID()) return false;
        }
        return true;
    }

    public static Transform[] DetachChildren(GameObject gameObject) {
        IncrementDetachCount(gameObject);
        Transform[] children = GetChildren(gameObject);
        gameObject.transform.DetachChildren();
        return children;
    }

    public static void ReattachChildren(Transform[] children, GameObject gameObject) {
        foreach (Transform T in children) {
            T.parent = gameObject.transform;
        }
        DecrementDetachCount(gameObject);
    }
}
