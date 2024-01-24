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
using System.IO;

namespace EternalLabyrinth
{
    class SmokeScreen : Ability, ScreenEffect
    {
        Smauken user;
        float distort;

        public SmokeScreen(Smauken user) : base(Game1.textures[T.SmokeScreen], new Rectangle(Game1.w / 2 - 1250, Game1.h / 2 - 1250, 2500, 2500), Color.White, true)
        {
            defaultAnimation = "SMOKE";
            currentAnimation = "SMOKE";
            this.user = user;
        }

        public override void LoadAnimations()
        {
            animations.Add("SMOKE", new Animation(20, 0, 0, 400, 400, 5, text.Width, true, false));
        }

        public override void Update()
        {
            base.Update();

            if (timeStamps.ContainsKey("SMOKESCREEN") && distort < 1.1)
            {
                distort += (float)0.1;
            }
            else if (timeStamps.ContainsKey("REMOVE") && Game1.Time - timeStamps["REMOVE"] > 300 && distort > 0)
            {
                distort -= (float)0.001;
            }
            else if (distort < 0 && timeStamps.ContainsKey("REMOVE"))
            {
                Game1.entities.Remove(this);
            }

            Player temp = (Player)Game1.entities[0];
            rect.X = (int)(temp.position.X) - rect.Width / 2;
            rect.Y = (int)(temp.position.Y) - rect.Height / 2;

            if (!HelperMethods.pixelCollision(user, Game1.entities[0]) && !timeStamps.ContainsKey("REMOVE"))
            {
                timeStamps["REMOVE"] = Game1.Time;
                timeStamps.Remove("SMOKESCREEN");
            }
        }

        public override void Use()
        {
            Game1.entities.Remove(this);
            Game1.entities.Insert(Game1.entities.Count - 1, this);
            timeStamps["SMOKESCREEN"] = Game1.Time;
            timeStamps.Remove("REMOVE");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(text, rect, currentFrame, Color.White * distort);
        }
    }
}
