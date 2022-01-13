using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2.Vertex
{
    public struct VertexPN : IVertexType
    {
        public Vector3 Pos;
        public Vector3 Normal;

        // int offset: offset in byte,
        // VertexElementFormat elementFormat: Format used to represent the variable
        // VertexElementUsage elementUsage: usage which will be used for
        // int usageIndex: index to name the element 

        public static readonly VertexDeclaration MyVertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            });


        public VertexPN(Vector3 pos, Vector3 normal)
        {
            Pos = pos;
            Normal = normal;
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return MyVertexDeclaration; }
        }
    }
}
