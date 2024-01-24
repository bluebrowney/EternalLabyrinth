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
    class Smauken : Enemy, PixelPerfectable, Moveable
    {
        // 32 x 32

        Dictionary<string, Ability> abilities;

        public Smauken(int x, int y, bool iFR) : base(Game1.textures[T.Smauken], new Rectangle(x, y, 32, 32), Color.White, iFR)
        {
            currentAnimation = "DEFAULT";
            defaultAnimation = "DEFAULT";
            speed = 0.5;
            abilities = new Dictionary<string, Ability>();
            LoadAbilities();
        }

        public override void LoadAnimations()
        {
            animations.Add("DEFAULT", new Animation(6, 0, 0, 32, 32, 5, text.Width, true, false));
            animations.Add("DEATH", new Animation(8, 0, 32, 32, 32, 5, text.Width, false, false));
        }

        public override void LoadAbilities()
        {
            abilities.Add("SMOKESCREEN", new SmokeScreen(this));
        }

        public override void Update()
        {
            base.Update();

            if (Game1.entities[0].rect.X < rect.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            if (HelperMethods.euclidianDistance(this, Game1.entities[0]) < 300)
            {
                int angle = HelperMethods.angleToObject(this, Game1.entities[0], Game1.entities[0].hitBox.Height / 4);
                movement.velocity.X = speed * Math.Cos(MathHelper.ToRadians(angle));
                movement.velocity.Y = speed * Math.Sin(MathHelper.ToRadians(angle));
            }

            if (HelperMethods.pixelCollision(this, Game1.entities[0]))
            {
                if (!timeStamps.ContainsKey("SMOKESCREEN"))
                {
                    abilities["SMOKESCREEN"].Use();
                    timeStamps["SMOKESCREEN"] = Game1.Time;
                }
            }
            else if (timeStamps.ContainsKey("SMOKESCREEN"))
            {
                timeStamps.Remove("SMOKESCREEN");
            }

            hitBox.X = movement.updatePosX();
            hitBox.Y = movement.updatePosY();

            rect.X = hitBox.X;
            rect.Y = hitBox.Y;
        }
    }
}
