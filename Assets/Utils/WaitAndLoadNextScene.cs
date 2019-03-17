using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ellada.TestTask.Utils {
    public class WaitAndLoadNextScene : MonoBehaviour {

        public float wait;

        public SceneItem nextScene;

        private IEnumerator Start() {
            yield return new WaitForSeconds(wait);
            Scenes.instance.LoadScene(nextScene);
        }
    }
}