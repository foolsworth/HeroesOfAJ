using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public abstract class ScriptableSingleton : ScriptableObject {
    protected static bool _Debug = false;
    public abstract void InitializeSingletonInstance ();
  }

  public abstract class ScriptableSingleton<T> : ScriptableSingleton where T : ScriptableSingleton<T> {
    private static T _Instance;
    // ReSharper disable once StaticMemberInGenericType
    private static bool _Initialized;

    public static T Instance {
      get {
        if (_Initialized) {
          return _Instance;
        }

        var type = typeof(T);
        T[] instances = GetAssetInstances ();
        if (instances == null || instances.Length == 0) {
          Debug.LogError ($"[ScriptableSingleton] No instance of {type} found!");
        } else if (instances.Length > 1) {
          Debug.LogError ($"[ScriptableSingleton] Multiple instances of {type} found!");
        } else {
          _Instance = instances[0];
          if (_Debug) {
            Debug.Log ($"[ScriptableSingleton] An instance of {type} type was found!");
          }
        }

        _Initialized = _Instance != null;
        return _Instance;
      }
    }

    private static T[] GetAssetInstances () {
      if (Application.isEditor) {
#if UNITY_EDITOR
        // So get all the assets of type T using AssetDatabase.
        string[] objsGuid = UnityEditor.AssetDatabase.FindAssets ("t:" + typeof(T).Name);
        int count = objsGuid.Length;
        var instances = new T[count];
        for (int i = 0; i < count; i++) {
          instances[i] =
            UnityEditor.AssetDatabase.LoadAssetAtPath<T> (
              UnityEditor.AssetDatabase.GUIDToAssetPath (objsGuid[i]));
        }

        return instances;
#endif
      }

      // Get all asset of type T from Resources or loaded assets.
      return Resources.FindObjectsOfTypeAll<T> ();
    }

    private void Awake () {
      InitializeSingletonInstance ();
    }

    protected virtual void OnEnable () {
      InitializeSingletonInstance ();
    }

    public override void InitializeSingletonInstance () {
      if (_Instance != null && _Instance != this) {
        Debug.LogError (
          $"{_Instance.GetType ()} instance is already assigned: {_Instance.name}. Ignoring duplicate: {name}");
        return;
      }

      _Instance = (T)this;
      _Initialized = _Instance != null;
    }
  }
