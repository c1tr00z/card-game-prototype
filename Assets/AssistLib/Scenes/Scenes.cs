using UnityEngine;
using System.Collections;

public class Scenes : MonoBehaviour {

    private SceneItem _currentSceneItem;
    private Scene _currentScene;

    private static Scenes _instance;

    public static Scenes instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
    }

    void Start() {
        LoadScene(AppSettings.instance.startScene);
    }

    public Scene LoadScene(SceneItem newScene) {
        return LoadScene(newScene, null);
    }

    public Scene LoadScene(SceneItem newScene, params object[] param) {
        if (_currentScene != null) {
            Destroy(_currentScene.gameObject);
        }

        _currentSceneItem = newScene;
        _currentScene = _currentSceneItem.GetScene();
        _currentScene.name = _currentSceneItem.name;
        _currentScene.transform.parent = transform;
        _currentScene.transform.localScale = Vector3.one;

        if (param != null && param.Length > 0) {
            _currentScene.SendMessage("OnLoadParams", param, SendMessageOptions.DontRequireReceiver);
        } else {
            _currentScene.SendMessage("OnLoad", SendMessageOptions.DontRequireReceiver);
        }
        

        return _currentScene;
    }
}
