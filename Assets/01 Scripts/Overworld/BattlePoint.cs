using UnityEngine.SceneManagement;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class BattlePoint : OverworldPoint
    {
        public string levelName = "";

        public override void Interact()
        {
            SceneManager.LoadScene(levelName);
        }
    }
}