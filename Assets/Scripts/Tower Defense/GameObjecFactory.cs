using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tower_Defense
{
    public abstract class GameObjecFactory : ScriptableObject
    {
        private Scene _scene;

        protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
        {
            if (!_scene.isLoaded)
            {
                if (Application.isEditor)
                {
                    _scene = SceneManager.GetSceneByName(name);
                    if (!_scene.isLoaded)
                    {
                        _scene = SceneManager.CreateScene(name);
                    }
                }
                else
                {
                    _scene = SceneManager.CreateScene(name);
                }
            }

            T instance = Instantiate(prefab);

            SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);

            return instance;
        }
    }
}