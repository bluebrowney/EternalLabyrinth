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
    public class Animation
    {
        public static readonly int SET_WIDTH = 832; //Fixed width for all spriteSheets for aniamtions
        public int duration
        {
            get
            {
                return (int)(((double)numberOfFrames / (double)framesPerSecond) * 60);
            }
        }
        List<Rectangle> frames; //source rectangles for aniamtion
        int framesPerSecond;
        public int numberOfFrames;
        public int currentFrame;
        public int startTime;
        public bool isLoop;
        public bool isDone;
        public bool isRestricting;
        public bool paused;
        public bool reversed;
        int pauseFrame = -1;

        public Animation(int fPS, int x, int y, int w, int h, int nOF, int fW, bool l, bool iR)
        {
            frames = new List<Rectangle>();
            framesPerSecond = fPS;
            numberOfFrames = nOF;
            isLoop = l;
            isRestricting = iR;
            setSourceRectangles(x, y, w, h, fW);
            isDone = false;
            paused = false;
            reversed = false;
            startTime = 0;
        }

        //Helping Setter for Source Rectangles
        private void setSourceRectangles(int sX, int sY, int width, int height, int fullWidth)
        {
            int frameCount = 0;
            for (int y = sY; frameCount < numberOfFrames; y += height)
            {
                for (int x = sX; x < fullWidth; x += width)
                {
                    frames.Add(new Rectangle(x, y, width, height));
                    frameCount++;
                    if(frameCount == numberOfFrames)
                    {
                        break;
                    }
                    sX = 0;
                }
            }
        }

        public void Play()
        {
            startTime = Game1.Time;
            currentFrame = 0;
            isDone = false;
        }

        public void Reverse()
        {
            isDone = false;
            reversed = !reversed;
        }
    
        public Rectangle getCurrentFrame()
        {
            if(currentFrame == pauseFrame)
            {
                paused = true;
            }
            int span = Game1.Time - startTime;
            if(span % (60 / framesPerSecond) == 0 && !isDone)
            {
                if(currentFrame == numberOfFrames - 1 && !reversed || currentFrame == 0 && reversed)
                {
                    if(isLoop)
                    {
                        if(reversed)
                        {
                            currentFrame = frames.Count - 1;
                        }
                        else
                        {
                            currentFrame = 0;
                        }
                    }
                    else
                    {
                        //return frames[currentFrame];
                        isDone = true;
                        return frames[currentFrame];
                    }
                }
                else
                {
                    if(!paused)
                    {
                        if(reversed)
                        {
                            currentFrame--;
                        }
                        else
                        {
                            currentFrame++;

                        }
                    }
                }
            }
            return frames[currentFrame];
        }

        public void Pause()
        {
            paused = true;
        }

        public void UnPause()
        {
            paused = false;
        }

        public void PauseAt(int frame)
        {
            pauseFrame = frame;
        }

        public void Reset()
        {
            startTime = 0;
            currentFrame = 0;
            isDone = false;
            paused = false;
            reversed = false;
        }

        public bool isPlaying()
        {
            if(startTime == 0)
            {
                return false;
            }
            return true;
        }

        //public int getElapsedTime()
        //{
        //    return HelperMethods.TotalMilliseconds(gameTime) - startTime; 
        //}
    }
}
