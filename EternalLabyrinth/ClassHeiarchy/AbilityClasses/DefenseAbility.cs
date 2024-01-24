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
    public class DefenseAbility : Ability, PixelPerfectable
    {
        public double reducedDamagePercent;
        int duration = 140;

        public DefenseAbility(Texture2D t, Rectangle r, Color c, bool iFR, int duration, double reducedDamagePercent) : base(t, r, c, iFR)
        {
            this.reducedDamagePercent = reducedDamagePercent;
            currentAnimation = "IDLE";
        }

        public void callAbility(int playerX, int playerY, bool iFR)
        {
            isCalled = true;
            rect.X = playerX;
            rect.Y = playerY;
            isFacingRight = iFR;

            motion = new Movement(rect);
        }
        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(1, 0, 0, text.Width, text.Height, 1, text.Width, true, false));

        }
        public override void Update()
        {
            base.Update();

            Player p = (Player)Game1.entities[0];
            hitBox = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            isFacingRight = p.isFacingRight;
            if (isCalled)
            {
                //p.damageReducer = reducedDamagePercent;
                if (isFacingRight)
                {
                    rect.X = p.rect.X + 40;
                    rect.Y = p.rect.Y;
                }
                else
                {
                    rect.X = p.rect.X;
                    rect.Y = p.rect.Y;
                }

                if (!timeStamps.ContainsKey("Shield"))
                {
                    timeStamps["Shield"] = HelperMethods.TotalSeconds();
                }
                else if (HelperMethods.TotalSeconds() - timeStamps["Shield"] == duration)
                {
                    isCalled = false;

                    //p.damageReducer = 1;
                    timeStamps.Remove("Shield");

                }
            }
        }
    }
}
