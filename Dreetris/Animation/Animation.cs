using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    public class KeyframeAnimation
    {
        List<Keyframe> keyframes = new List<Keyframe>();
        private int currentFrameIndex;
        bool _finished;

        public Keyframe CurrentFrame
        {
            get
            {
                return keyframes[currentFrameIndex];
            }
        }

        public int Index
        {
            get { return currentFrameIndex; }
        }

        public void AddKeyframe(Keyframe keyframe)
        {
            keyframes.Add(keyframe);
        }

        public int GetX()
        {
            return (int)CurrentFrame.Current.X;
        }

        public int GetY()
        {
            return (int)CurrentFrame.Current.Y;
        }

        public void Update(GameTime gameTime)
        {
            if (!_finished)
            {
                CurrentFrame.Update(gameTime);
                var time = CurrentFrame.RunningTime;
                if (time <= 0)
                {
                    currentFrameIndex++;
                    if (currentFrameIndex >= keyframes.Count)
                    {
                        _finished = true;
                        currentFrameIndex = 0;
                        CurrentFrame.Reset();
                    }
                    else
                    {
                        //TODO: As long as necessary. Maybe more than one keyframe has to be skipped
                        CurrentFrame.Delay(time);
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
