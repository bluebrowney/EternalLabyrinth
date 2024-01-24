using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace EternalLabyrinth
{
    //Allows objects to collide in Game1.cs
    interface PixelPerfectable
    {
        Color[] getColorData(); // returns the required pixel data;
        Rectangle getFrame();
    }
}
