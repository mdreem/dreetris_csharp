using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    class Animation
    {
        List<Keyframe> keyframes = new List<Keyframe>();
        int current_frame_index = 0;
        bool _finished = false;

        public Keyframe current_frame
        {
            get
            {
                return keyframes[current_frame_index];
            }
        }

        public int index
        {
            get { return current_frame_index; }
        }

        public void add_keyframe(Keyframe keyframe)
        {
            keyframes.Add(keyframe);
        }

        public int get_x()
        {
            return (int)current_frame.current.X;
        }

        public int get_y()
        {
            return (int)current_frame.current.Y;
        }

        public void Update(GameTime gameTime)
        {
            if (!_finished)
            {
                current_frame.Update(gameTime);
                var time = current_frame.running_time;
                if (time <= 0)
                {
                    current_frame_index++;
                    if (current_frame_index >= keyframes.Count)
                    {
                        _finished = true;
                        current_frame_index = 0;
                        current_frame.reset();
                    }
                    else
                    {
                        //current_frame.init(gameTime);
                        current_frame.delay(time); //TODO: As long as necessary. Maybe more than one keyframe has to be skipped
                    }
                }
            }
        }

        public List<Vector2> get_path()
        {
            List<Vector2> res = new List<Vector2>();

            foreach (var path in keyframes)
            {
                res.AddRange(path.get_path());
            }

            return res;
        }
    }
}
