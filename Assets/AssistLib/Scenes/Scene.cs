using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    [SerializeField] private UIFrameItem _associatedUIFrame;

    void Start() {
        if (_associatedUIFrame != null) {
            _associatedUIFrame.Show();
        }
    }
	
}
