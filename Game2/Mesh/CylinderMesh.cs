using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game2
{
    public class CylinderMesh
    {
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;
       

        public Effect effect;
        public Texture2D texture;

        public Vector3 LightVecW = new Vector3(1.0f, 0.0f, 0.0f);

        GraphicsDevice graphicsDevice;

        uint grid = 64;

        float scaleZ = 1;
        float scaleX = 0.5f;
        bool buildVertex = true;

        public VertexPNT[] vertices;

        public CylinderMesh(GraphicsDevice graphicsDevice, Effect effect, Texture2D texture, Boolean buildVertex = true)
        {
            this.graphicsDevice = graphicsDevice;

            this.effect = effect;
            this.texture = texture;

            vertices = new VertexPNT[grid * 2 + 2];

            this.buildVertex = buildVertex;
            BuildVertexBuffer();
            BuildIndexBuffer();
        }

        public void Transform(Matrix matrix)
        {
            for (uint i = 0; i < vertices.Length; i++)
            {
                vertices[i].pos = Vector3.Transform(vertices[i].pos, matrix);
            }

            LightVecW = Vector3.TransformNormal(LightVecW, matrix);

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPNT), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPNT>(vertices);
        }

        public void BuildVertexBuffer()
        {
            for (uint i = 0; i < grid; i++)
            {
                uint index = i;
                uint indexb1 = i + grid;

                double theta =  i * 2*Math.PI / (grid - 1);


                vertices[i].pos.X = scaleX * (float)Math.Cos(theta);
                vertices[i].pos.Y = -scaleZ;
                vertices[i].pos.Z = scaleX * (float)Math.Sin(theta);

                vertices[i].tex0.X = -((float)i) / (grid - 1);
                vertices[i].tex0.Y = 1;


                vertices[i+grid].pos.X = scaleX * (float)Math.Cos(theta);
                vertices[i+grid].pos.Y = scaleZ;
                vertices[i+grid].pos.Z = scaleX * (float)Math.Sin(theta); 

                vertices[i+grid].tex0.X = -((float)i) / (grid - 1); ;
                vertices[i+grid].tex0.Y = 0;


                Vector3 normal = new Vector3(vertices[i].pos.X, 0.0f, vertices[i].pos.Z);
                vertices[i].normal = normal;
                vertices[i + grid].normal = normal;

            }

            vertices[grid * 2].pos.Y = -scaleZ;
            vertices[grid * 2].normal.Y = -1.0f;
            vertices[grid * 2].tex0.Y = 1.0f;

            vertices[grid * 2 + 1].pos.Y = scaleZ;
            vertices[grid * 2 + 1].normal.Y = 1.0f;
            vertices[grid * 2 + 1].tex0.Y = 0.0f;

            if (buildVertex)
            {
                vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPNT), vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPNT>(vertices);
            }
        }

        public void BuildIndexBuffer()
        {
            short[] indices = new short[(grid - 1) * 2 * 3 + (grid - 1) * 2 * 3];

            uint current = 0;
            for (uint i = 0; i < grid - 1; i++)
            {
                short indexA1 = (short)i;
                short indexA2 = (short)(i + 1);

                short indexB1 = (short)(i + grid);
                short indexB2 = (short)(i + grid + 1);

                indices[current] = indexA1;
                indices[current + 1] = indexA2;
                indices[current + 2] = indexB2;
                current += 3;

                indices[current] = indexA1;
                indices[current + 1] = indexB2;
                indices[current + 2] = indexB1; 
                current += 3;
            }

            //current = (grid - 1) * 2 * 3;
            for (uint i = 0; i < grid - 1; i++)
            {
                indices[current] = (short)i;
                indices[current+1] = (short)(i + 1);
                indices[current+2] = (short)(grid * 2);
                current += 3;
            }

            for (uint i = 0; i < grid - 1; i++)
            {
                indices[current] = (short)(i + grid);
                indices[current + 1] = (short)(i + grid + 1);
                indices[current + 2] = (short)(grid * 2 + 1);
                current += 3;
            }

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
    }
}
