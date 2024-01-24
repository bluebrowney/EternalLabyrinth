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

    class Button
    {
        SpriteFont Font1;
        Rectangle bpos;
        bool click;
        Texture2D blank;
        Color col;
        Color col2;
        int pady;
        int padx;
        string type;
        public Button()
        {
            bpos = new Rectangle(100, 100, 100, 50);
            click = false;
            col = Color.WhiteSmoke;
            col2 = Color.Black;
            type = "Default";
            pady = 0;
            padx = 0;
        }
        public Button(Rectangle pos, bool clicked, string s)
        {
            bpos = pos;
            click = clicked;
            col2 = Color.Black;
            type = s;
            pady = 0;
            padx = 0;

        }

        public void LoadContent(ContentManager content)
        {
            blank = content.Load<Texture2D>("white");
            Font1 = content.Load<SpriteFont>("SpriteFont1");
            ButtonCentering();
        }

        public void Update(MouseState m, MouseState oldm)
        {

            if ((m.X > bpos.X && m.X < bpos.X + bpos.Width && (m.Y > bpos.Y && m.Y < bpos.Y + bpos.Height)))
            {
                col = Color.Yellow;
                if (m.LeftButton == ButtonState.Pressed && oldm != m)
                    click = true;
            }
            else
            {
                col = Color.WhiteSmoke;
            }

        }
        public void ButtonCentering()
        {
            if (bpos.Width < Font1.MeasureString(type).X)
                bpos.Width = (int)Font1.MeasureString(type).X + 20;
            else if (bpos.Height < Font1.MeasureString(type).Y)
                bpos.Height = (int)Font1.MeasureString(type).Y + 20;

            padx = (bpos.Width - (int)Font1.MeasureString(type).X) / 2;
            pady = (bpos.Height - (int)Font1.MeasureString(type).Y) / 2;

        }
        public void Draw(SpriteBatch s)
        {

            //     s.Begin();
            s.Draw(blank, bpos, col);
            s.DrawString(Font1, type, new Vector2(bpos.X + (padx), bpos.Y + (pady)), col2, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            //   s.End();

        }
        public void setTexture(Texture2D t)
        {
            blank = t;
            col2 = Color.White;

        }
        public void setColor(Color c)
        {

            col2 = c;

        }
        public bool isPressed()
        {
            return click;
        }




    }
}