using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class UI_TowerHP : UI_Scene
    {
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, false, -1);
        }
    }
}
