using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace EternalLabyrinth
{

    //Base Class for all objects in the game to decrease code present in Game1.cs : Zaaim
    public class Object
    {
        //All objects have a location rectangle, texture, color, and bool which determines the way it will face. : Zaaim
        public Rectangle rect;
        public Texture2D text;
        public Color col;
        public bool isFacingRight;
        public Color baseColor;
        public Rectangle hitBox;
        public Dictionary<string, int> timeStamps;

        public Object(Texture2D t, Rectangle r, Color c)
        {
            rect = r;
            hitBox = rect;
            text = t;
            col = c;
        }

        public Object()
        {
            rect = new Rectangle();
            col = Color.White;
            isFacingRight = true;
        }

        //Visual Editing : Zaaim
        //Abstact method overloaded by subclasses
        public virtual void Draw(SpriteBatch spriteBatch) { }

        //Abstact method overloaded by subclasses
        public virtual void Update() { }

        //Updates the col of the object
        public void updateColor(Color c)
        {
            col = c;
        }
    }
}
