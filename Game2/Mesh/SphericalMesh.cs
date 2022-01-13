using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game2
{

    public class SphericalMesh
    {
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public Effect effect;
        public Texture2D texture;

        public Vector3 LightVecW = new Vector3(1.0f, 0.0f, 0.0f);

        GraphicsDevice graphicsDevice;

        public float Scale = 1.0f;

        uint grid = 64;

        public SphericalMesh(GraphicsDevice graphicsDevice, Effect effect, Texture2D texture)
        {
            this.graphicsDevice = graphicsDevice;

            this.effect = effect;
            this.texture = texture;

            BuildVertexBuffer();
            BuildIndexBuffer();
        }

        public void BuildVertexBuffer()
        {
            VertexPNT[] vertices = new VertexPNT[grid*grid+2];
            
            for (uint i = 0; i < grid; i++)
            {
                for (uint j = 0; j < grid; j++)
                {
                    uint index = i * grid + j;

                    float theta = (float)(j * (2*Math.PI)/ grid);
                    float phi   = (float)((i * (Math.PI - (Math.PI / grid))/ grid) + Math.PI/grid);

                    vertices[index].pos.X = (float)(Math.Cos(theta) * Math.Sin(phi) * Scale);
                    vertices[index].pos.Z = (float)(Math.Sin(theta) * Math.Sin(phi) * Scale);
                    vertices[index].pos.Y = (float)(Math.Cos(phi) * Scale); 

                    float u = -theta / (2.0f * (float)Math.PI);
                    float v = phi / (float)Math.PI;

                    vertices[index].tex0.X = u;
                    vertices[index].tex0.Y = v;
                    vertices[index].normal = vertices[index].pos;
                }
            }
            vertices[grid*grid].normal.Y = 1.0f;
            vertices[grid*grid].pos.Y = Scale;
            vertices[grid*grid].tex0 = new Vector2(0, 0);

            vertices[grid*grid+1].normal.Y = -1.0f;
            vertices[grid*grid+1].pos.Y = -Scale;
            vertices[grid*grid+1].tex0 = new Vector2(1.0f, 1.0f);

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPNT), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPNT>(vertices);
        }

        public void BuildIndexBuffer()
        {
            // 8 * 8 * 2 triangles
            // 7 * 8 * 2 for the center, 8 + 8 for ends
            short[] indices = new short[(grid-1)*grid*2*3 + grid*3*2];
            
            for (uint i = 0; i < grid-1; i++)
            {
                for (uint j = 0; j < grid; j++)
                {
                    short indexa2 = (short)((j + 1) % grid + i * grid);
                    short indexb1 = (short)(j % grid + (i+1) * grid);
                    short indexb2 = (short)((j + 1) % grid + (i+1) * grid);

                    short index = (short)(i * grid + j);
                    uint indexTriangle = (i * grid + j)*3*2;
                    indices[indexTriangle] = index;
                    indices[indexTriangle + 1] = indexb1;
                    indices[indexTriangle + 2] = indexb2;

                    indexTriangle += 3;
                    indices[indexTriangle] = index;
                    indices[indexTriangle + 1] = indexb2;
                    indices[indexTriangle + 2] = indexa2;
                }
            }

            uint max = (grid - 1) * grid * 2 * 3;

            for (uint i = 0; i < grid; i++)
            {
                short indexa1 = (short)i;
                short indexa2 = (short)((i + 1) % grid);

                indices[max] = indexa1;
                indices[max + 1] = indexa2;
                indices[max + 2] = (short)(grid * grid);
                max += 3;
            }

            for (uint i = 0; i < grid; i++)
            {
                short indexa1 = (short)(grid*(grid-1) + i);
                short indexa2 = (short)(grid*(grid-1) + (i+1) % grid);

                indices[max] = indexa1;
                indices[max + 1] = indexa2;
                indices[max + 2] = (short)(grid * grid + 1);
                max += 3;
            }

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

    }
}
