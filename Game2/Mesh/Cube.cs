using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2
{
    public class Cube
    {
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public Effect cubeEffect;
        public Effect diffuseEffect;

        public Vector4 AmbientMtrl = new Vector4(0.3f, 0.2f, 0.3f, 1.0f);
        public Vector4 AmbientLight = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
        public Vector4 DiffuseMtrl = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
        public Vector4 DiffuseLight = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);
        public Vector3 LightVecW = new Vector3(1.0f, 0.0f, 0.0f);

        GraphicsDevice graphicsDevice;


        public Cube(GraphicsDevice graphicsDevice, Effect effect, Effect diffuseEffect)
        {
            this.graphicsDevice = graphicsDevice;

            this.cubeEffect = effect;
            this.diffuseEffect = diffuseEffect;

            BuildVertexBufferColor();
            BuildIndicesBuffer();
        }

        public void BuildVertexBuffer()
        {
            VertexPN[] vertices = new VertexPN[8];
            
            vertices[0] = new VertexPN(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            vertices[1] = new VertexPN(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(1.0f, -1.0f, 1.0f));
            vertices[2] = new VertexPN(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(-1.0f, 1.0f, 1.0f));
            vertices[3] = new VertexPN(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1.0f, 1.0f, -1.0f));
            vertices[4] = new VertexPN(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(1.0f, -1.0f, 1.0f));
            vertices[5] = new VertexPN(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            vertices[6] = new VertexPN(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, -1.0f, -1.0f));
            vertices[7] = new VertexPN(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(-1.0f, -1.0f, 1.0f));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPN), 8, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPN>(vertices);
        }

        public void BuildVertexBufferColor()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[8];

            vertices[0] = new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Blue);
            vertices[2] = new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Green);
            vertices[3] = new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Red);
            vertices[4] = new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Blue);
            vertices[5] = new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.Green);
            vertices[6] = new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Red);
            vertices[7] = new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Blue);

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
        }


        public void BuildIndicesBuffer()
        {
            short[] k = new short[36];

            // Front face
            k[0] = 0; k[1] = 1; k[2] = 2;
            k[3] = 0; k[4] = 2; k[5] = 3;

            // Back face
            k[6] = 4; k[7] = 6; k[8] = 5;
            k[9] = 4; k[10] = 7; k[11] = 6;

            // Left face
            k[12] = 4; k[13] = 5; k[14] = 1;
            k[15] = 4; k[16] = 1; k[17] = 0;

            // Right face
            k[18] = 3; k[19] = 2; k[20] = 6;
            k[21] = 3; k[22] = 6; k[23] = 7;

            // Top face
            k[24] = 1; k[25] = 5; k[26] = 6;
            k[27] = 1; k[28] = 6; k[29] = 2;

            // Bottom face
            k[30] = 4; k[31] = 0; k[32] = 3;
            k[33] = 4; k[34] = 3; k[35] = 7;

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), k.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(k);
        }



    }
}
