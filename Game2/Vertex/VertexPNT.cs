using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2.Vertex
{
    public struct VertexPNT : IVertexType
    {
        public Vector3 pos;
        public Vector3 normal;
        public Vector2 tex0;

        public VertexPNT(Vector3 pos, Vector3 normal, Vector2 tex0)
        {
            this.pos = pos;
            this.normal = normal;
            this.tex0 = tex0;
        }

        public static readonly VertexDeclaration MyVertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            });

        public VertexDeclaration VertexDeclaration
        {
            get { return MyVertexDeclaration; }
        }
    }
}
