using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.System
{
    public static class Properties
    {
        public static float TEXT_HEIGHT = 18;
        public static GameType ActiveGameType = GameType.SinglePlayer;

        public static float BackButtonSize;
        public static UnityEngine.Texture2D BackButton,MenuButton;  
    }

    public enum GameType{
        SinglePlayer,
        MultiPlayer
    }
}
