using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EternalLabyrinth
{
    public class Tile : StaticObject, PixelPerfectable
    {
        public String textString;
        public Boolean foreground, hazard;
        public Rectangle sourceRect;

        public Tile() { textString = ""; }

        public Tile(char ch) : base()
        {
            text = Game1.tileSheet;
            textString = "";
            switch (ch)
            {
                //MISC TILES
                case 'x':
                    col = Color.White;
                    break;
                case '.':
                    col = Color.Black;
                    break;
                case 'a':
                    col = Color.White;
                    textString = "dirt";
                    break;
                case 'b':
                    col = Color.White;
                    textString = "crate";
                    break;
                case 'c':
                    col = Color.White;
                    textString = "wall";
                    break;
                case 'd':
                    col = Color.White;
                    textString = "stone";
                    break;
                case 'e':
                    col = Color.White;
                    textString = "rock";
                    hazard = true;
                    break;

                //FOREGROUND TILES
                case 'g':
                    col = Color.White;
                    textString = "grass";
                    foreground = true;
                    break;
                case 'j':
                    col = Color.White;
                    textString = "jail";
                    foreground = true;
                    break;
                case 'k':
                    col = Color.White;
                    textString = "cams";
                    foreground = true;
                    break;
                case 'h':
                    col = Color.White;
                    textString = "vine1";
                    foreground = true;
                    break;
                case 'i':
                    col = Color.White;
                    textString = "vine2";
                    foreground = true;
                    break;
                case 'R':
                    col = Color.White;
                    textString = "stalactite";
                    foreground = true;
                    break;
                case 'S':
                    col = Color.White;
                    textString = "stalagmite";
                    foreground = true;
                    break;
                case 'T':
                    col = Color.White;
                    textString = "stalagmiteBlue";
                    foreground = true;
                    break;
                case 'U':
                    col = Color.White;
                    textString = "stalactiteBlue";
                    foreground = true;
                    break;
                case 'V':
                    col = Color.White * 0.5f;
                    textString = "waterHalf";
                    foreground = true;
                    break;
                case 'W':
                    col = Color.White * 0.5f;
                    textString = "waterFull";
                    foreground = true;
                    break;

                //BLUE TILES
                case 'l':
                    col = Color.White;
                    textString = "blueDirtVine";
                    break;
                case 'm':
                    col = Color.White;
                    textString = "blueStoneVine";
                    break;
                case 'n':
                    col = Color.White;
                    textString = "bluePattWall";
                    break;
                case 'o':
                    col = Color.White;
                    textString = "blueCrate";
                    break;
                case 'r':
                    col = Color.White;
                    textString = "blueStoneWall";
                    break;
                case 'A':
                    col = Color.White;
                    textString = "blueSpike";
                    hazard = true;
                    break;

                //RED TILES
                case 's':
                    col = Color.White;
                    textString = "redStoneWall";
                    break;
                case 't':
                    col = Color.White;
                    textString = "redCrate";
                    break;
                case 'v':
                    col = Color.White;
                    textString = "redStoneVine";
                    break;
                case 'C':
                    col = Color.White;
                    textString = "redSpike";
                    hazard = true;
                    break;
                //YELLOW TILES
                case 'z':
                    col = Color.White;
                    textString = "yellowDirt";
                    break;
                case 'B':
                    col = Color.White;
                    textString = "yellowSpike";
                    hazard = true;
                    break;
                case 'f':
                    col = Color.White;
                    textString = "yellowStoneWall";
                    break;
                case 'O':
                    col = Color.White;
                    textString = "yellowTree";
                    break;

                //LOGIC TILES
                case 'p':
                    textString = "player";
                    break;
                case 'u':
                    col = Color.White;
                    textString = "skeltri";
                    foreground = true;
                    break;
                case 'D':
                    col = Color.White;
                    textString = "bouldren";
                    foreground = true;
                    break;
                case 'E':
                    col = Color.White;
                    textString = "flame";
                    foreground = true;
                    break;
                case 'F':
                    col = Color.White;
                    textString = "skull";
                    foreground = true;
                    break;
                case 'G':
                    col = Color.White;
                    textString = "smauken";
                    foreground = true;
                    break;
                case 'H':
                    col = Color.White;
                    textString = "waterne";
                    foreground = true;
                    break;
                case 'y':
                    col = Color.White * .1f;
                    textString = "death";
                    foreground = true;
                    break;
                case 'q':
                    col = Color.White;
                    textString = "nextLevel";
                    foreground = true;
                    break;
            }
        }

        public void setRect(Rectangle src)
        {
            sourceRect = src;
        }
    }
}

