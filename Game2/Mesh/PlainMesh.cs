using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game2
{
    public class PlainMesh
    {
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;
        public VertexPNT[] vertices = new VertexPNT[4];
        public Vector3 normal = new Vector3(1.0f, 0, 0);
        public Vector3 position = new Vector3(5.0f, 0, 0);
        public float ScaleY = 3;
        public float ScaleZ = 2;

        public Effect effect;

        public PlainMesh(GraphicsDevice graphicsDevice, Effect effect, Vector3 position, Vector3 normal, float scaleY, float scaleZ)
        {
            this.effect = effect;

            if (normal != null)
            {
                this.normal = normal;
            }

            if (position != null)
            {
                this.position = position;
            }

            this.ScaleY = scaleY;
            this.ScaleZ = scaleZ;

            BuildVertexBuffer(graphicsDevice);
            BuildIndicesBuffer(graphicsDevice);
        }

        public Plane GetPlane()
        {
            return new Plane(vertices[0].pos, vertices[1].pos, vertices[2].pos);
        }

        public void BuildVertexBuffer(GraphicsDevice graphicsDevice)
        {
            normal.Normalize();
            Vector3 up;
            if (normal.X != 0f || normal.Y != 0f)
            {
                up = new Vector3(-normal.Y, normal.X, 0);
            }
            else
            {
                up = new Vector3(-normal.Z, normal.X, 0);
            }
            float res = Vector3.Dot(normal, up);
            Debug.Assert(res == 0);
            Vector3 left = Vector3.Cross(up, normal);

            vertices[0] = new VertexPNT(position  - left * ScaleZ - ScaleY * up, normal, new Vector2(0, 0));
            vertices[1] = new VertexPNT(position  - left * ScaleZ + ScaleY * up, normal, new Vector2(0, 1));
            vertices[2] = new VertexPNT(position + left * ScaleZ + ScaleY * up, normal, new Vector2(1, 1));
            vertices[3] = new VertexPNT(position + left * ScaleZ - ScaleY * up, normal, new Vector2(1, 0));


            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPNT), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPNT>(vertices);
        }

        public void BuildIndicesBuffer(GraphicsDevice graphicsDevice)
        {
            short[] k = new short[6];

            // Front face
            k[0] = 0; k[1] = 1; k[2] = 2;
            k[3] = 0; k[4] = 2; k[5] = 3;

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), k.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(k);
        }
    }
}
