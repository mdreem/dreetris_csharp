using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    public class KeyframeAnimation
    {
        List<Keyframe> keyframes = new List<Keyframe>();
        int currentFrameIndex = 0;
        bool _finished = false;

        public Keyframe currentFrame
        {
            get
            {
                return keyframes[currentFrameIndex];
            }
        }

        public int index
        {
            get { return currentFrameIndex; }
        }

        public void addKeyframe(Keyframe keyframe)
        {
            keyframes.Add(keyframe);
        }

        public int getX()
        {
            return (int)currentFrame.current.X;
        }

        public int getY()
        {
            return (int)currentFrame.current.Y;
        }

        public void Update(GameTime gameTime)
        {
            if (!_finished)
            {
                currentFrame.Update(gameTime);
                var time = currentFrame.runningTime;
                if (time <= 0)
                {
                    currentFrameIndex++;
                    if (currentFrameIndex >= keyframes.Count)
                    {
                        _finished = true;
                        currentFrameIndex = 0;
                        currentFrame.Reset();
                    }
                    else
                    {
                        currentFrame.Delay(time); //TODO: As long as necessary. Maybe more than one keyframe has to be skipped
                    }
                }
            }
        }

        public List<Vector2> GetPath()
        {
            List<Vector2> res = new List<Vector2>();

            foreach (var path in keyframes)
            {
                res.AddRange(path.GetPath());
            }
            return res;
        }
    }
}
