using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    class Animation
    {
        List<Keyframe_Straight> keyframes = new List<Keyframe_Straight>();
        int current_frame_index = 0;
        bool _finished = false;

        public Keyframe_Straight current_frame
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

        public void add_keyframe(Keyframe_Straight keyframe)
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
    }
}
