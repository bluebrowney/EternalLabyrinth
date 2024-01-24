using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace EternalLabyrinth
{
    public class StaticObject : Object
    {
        //Source rectangle to grab from sheet (set to entire texture if it doesnt require one)
        Rectangle sourceRect;

        public StaticObject(Texture2D t, Rectangle r, Rectangle sR, Color c) : base(t, r, c)
        {
            sourceRect = sR;
        }

        public StaticObject() : base()
        {
            sourceRect = new Rectangle(0, 0, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(text, rect, sourceRect, col);
            //spriteBatch.Draw(Game1.white, hitBox, sourceRect, Color.Green * 0.4f);
        }

        //FOR COLLISIONS
        public Color[] getColorData()
        {
            //CHANGE BACK TO SOURCERECT LATER
            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            this.text.GetData(0, sourceRect, data, 0, data.Length);
            if (isFacingRight)
            {
                return data;
            }
            else
            {
                Array.Reverse(data);
                return data;
            }
        }

        //FOR COLLISIONS
        public Rectangle getFrame()
        {
            return sourceRect;
        }
    }
}
