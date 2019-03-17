using UnityEngine;
using System.Collections;

public class SceneItem : DBEntry {

    public Scene GetScene() {
        return this.LoadPrefab<Scene>().Clone();
    }
}
