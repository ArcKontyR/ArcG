﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public interface IDrawable
    {
        public void Draw(SpriteBatch spriteBatch);
    }
}
