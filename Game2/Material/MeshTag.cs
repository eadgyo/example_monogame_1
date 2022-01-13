using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2
{
    public class MeshTag
    {
        public Vector4 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        public MeshTag(Vector4 color, Texture2D texture, float SpecularPower)
        {
            this.Color = color;
            this.Texture = texture;
            this.SpecularPower = SpecularPower;
        }

    }
}
