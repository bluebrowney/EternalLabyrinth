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
    class SkeltriStomp : Ability, PixelPerfectable
    {
        //readonly int width = 32;
        //readonly int height = 32;

        //public readonly int maxDamage = 6;
        public int attackDuration = 180;
        
        //int duration = 180;

        Skeltri user;

        public SkeltriStomp(Skeltri user) : base(Game1.textures[T.SkeltriStomp], new Rectangle(user.rect.X, user.rect.Bottom + 30, 180, 30), Color.White, user.isFacingRight)
        {
            this.user = user;
            currentAnimation = "STOMP";
            defaultAnimation = "STOMP";
            timeStamps = new Dictionary<string, int>();
        }

        public override void LoadAnimations()
        {
            animations.Add("STOMP", new Animation(5, 0, 0, 180, 30, 4, text.Width, false, false));
            //animations["STOMP"].Reverse();
        }

        public override void Update()
        {
            base.Update();

            int check = animations["STOMP"].currentFrame;
            int dood = 5;

            if(timeStamps.ContainsKey("HIT"))
            {
                timeStamps["HIT"] += 1;
                if(timeStamps["HIT"] >= attackDuration)
                {
                    animations["STOMP"].Reverse();
                    animations["STOMP"].UnPause();
                    timeStamps.Remove("HIT");
                    timeStamps["RECALL"] = 1;
                }
            }

            if(Game1.entities[0] is Player)
            {
                if(!timeStamps.ContainsKey("HIT") && ! timeStamps.ContainsKey("RECALL") && HelperMethods.pixelCollision(this, Game1.entities[0]))
                {
                    timeStamps["HIT"] = 0;
                    animations["STOMP"].Pause();
                    ((Player)Game1.entities[0]).Immobilize(attackDuration);
                }
            }
            

            if (animations["STOMP"].paused && !timeStamps.ContainsKey("HIT"))
            {
                animations["STOMP"].Reverse();
                animations["STOMP"].UnPause();
                timeStamps["RECALL"] = 1;
            }

            if (animations["STOMP"].currentFrame == animations["STOMP"].numberOfFrames - 1 && !timeStamps.ContainsKey("RECALL") && !timeStamps.ContainsKey("HIT"))
            {
                animations["STOMP"].Pause();
            }

            if (animations["STOMP"].reversed && animations["STOMP"].currentFrame == 0)
            {
                Game1.entities.Remove(this);
                animations["STOMP"].Reset();
                timeStamps.Remove("USING");
                timeStamps.Remove("RECALL");
                timeStamps.Remove("HIT");
            }

        }    

            



        public override void Use()
        {
            if(!timeStamps.ContainsKey("USING"))
            {
                PlayAnimation("STOMP");
                if (!user.isFacingRight)
                {
                    rect.X = user.rect.X - rect.Width;
                    rect.Y = user.rect.Bottom - rect.Height;
                    isFacingRight = !user.isFacingRight;
                }
                else //Change
                {
                    rect.X = user.rect.X + user.rect.Width;
                    rect.Y = user.rect.Bottom - rect.Height;
                    isFacingRight = !user.isFacingRight;
                }
                timeStamps["USING"] = 1;
                Game1.entities.Add(this);
            }
        }
    }
}
