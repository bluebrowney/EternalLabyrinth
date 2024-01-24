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
    class FireBall : Ability, PixelPerfectable, Moveable
    {
        public FireBall(Player user) : base(Game1.textures[T.FireBall], new Rectangle(user.rect.X, user.rect.Y, 32, 32), Color.White, user.isFacingRight)
        {

        }

        public Movement getMotion()
        {
            return motion;
        }
    }
}
