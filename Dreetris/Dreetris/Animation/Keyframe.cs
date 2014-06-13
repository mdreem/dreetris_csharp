using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    public abstract class Keyframe
    {
        public abstract void Update(GameTime gameTime);
        public abstract void reset();
    }
}
