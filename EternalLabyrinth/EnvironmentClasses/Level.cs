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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
   

    public class Level
    {

        public Tile[,] tiles;
        public bool isBackground;

        public bool isPlayerMiddle;
        int bufferAdjust = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 



        public void Update(GameTime gameTime)
        {
            int velocity;
            Player player = (Player)Game1.entities[0];
            setIsPlayerMiddle(player);


            if (isPlayerMiddle)
                velocity = (int)player.motion.velocity.X * -1;
            else
                velocity = 0;



            if (!isBackground)
            {
                if (isPlayerMiddle)
                {
                    player.motion.position.X -= player.motion.velocity.X;
                    player.motion.position.X += bufferAdjust;
                }
                foreach (Object o in Game1.entities)
                {
                    if (o is Moveable && !(o is Player))
                    {
                        Moveable temp = (Moveable)o;
                        Movement movTemp = temp.getMotion();
                        movTemp.position.X += velocity;
                        movTemp.position.X += bufferAdjust;
                    }
                    else if ((o is SmokeScreen))
                    {
                    }
                    else if (!(o is Player))
                    {
                        o.rect.X += velocity;
                        o.rect.X += bufferAdjust;
                        o.hitBox = o.rect;
                    }

                }
            }
            else
            {
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        if (tiles[i, j] != null)
                        {
                            tiles[i, j].rect.X += velocity;
                            tiles[i, j].rect.X += bufferAdjust;
                            tiles[i, j].hitBox = tiles[i, j].rect;
                        }
                    }
                }
            }
            
        }

        public void setIsPlayerMiddle(Player player)
        {
            if (player.rect.X < 10)
            {
                player.rect.X = 10;
            }
            else if (player.rect.X > 1250 - player.rect.Width)
            {

                player.rect.X = 1250 - player.rect.Width;
            }

            // TODO: Add your update logic here
            if (/*Game1.presIn.Contains(Keys.Right) || */player.motion.velocity.X > 0)
            {

                if (((player.rect.X < (Game1.w - player.rect.Width) / 2) || tiles[0, 0].rect.X == 0) && !(player.rect.X >= (Game1.w - player.rect.Width) / 2))
                {
                    isPlayerMiddle = false;
                    bufferAdjust = 0;
                }
                else if (tiles[0, 0].rect.X <= -2560)
                {
                    isPlayerMiddle = false;
                    if (tiles[0, 0].rect.X < -2560)
                    {
                        bufferAdjust = -2560 - tiles[0, 0].rect.X;
                    }
                    else
                    {
                        bufferAdjust = 0;
                    }
                }
                else
                {
                    bufferAdjust = 0;
                    isPlayerMiddle = true;
                }



            }

            //Controls left background movement
            else if (/*Game1.presIn.Contains(Keys.Left) || */player.motion.velocity.X < 0)
            {
                if (tiles[0, 0].rect.X >= 1200 || !(player.rect.X <= (Game1.w - player.rect.Width) / 2))
                {
                    isPlayerMiddle = false;
                    bufferAdjust = 0;
                }
                else if (tiles[0, 0].rect.X >= 0)
                {
                    isPlayerMiddle = false;
                    if(tiles[0, 0].rect.X > 0)
                    {
                        bufferAdjust = 0 - tiles[0, 0].rect.X;
                    }
                    else
                    {
                        bufferAdjust = 0;
                    }
                }
                else
                {
                    bufferAdjust = 0;
                    isPlayerMiddle = true;
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // TODO: Add your drawing code here


            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        if (isBackground)
                        {
                            spriteBatch.Draw(Game1.tileSheet, tiles[i, j].rect, tiles[i, j].sourceRect, Color.DimGray);
                        }
                        else
                        {
                            spriteBatch.Draw(Game1.tileSheet, tiles[i, j].rect, tiles[i, j].sourceRect, tiles[i, j].col);
                        }
                    }

                }
            }
        }
       
        
    }
}

