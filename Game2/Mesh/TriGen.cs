using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2
{
    class TriGen
    {
        public VertexBuffer vertexBufferGrid;
        public IndexBuffer indexBufferGrid;

        public Effect waveEffect;
        public Effect multiTextureEffect;
        public Effect textureEffect;

        GraphicsDevice graphicsDevice;
        public Texture2D texture;
        public Texture2D texture2;
        public Texture2D blendMap;
        public Texture2D normal;

        public Vector3 LightVecW = new Vector3(1.0f, 0.0f, 0.0f);

        public List<Vector3> verts = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();


        public List<short> indices = new List<short>();

        public TriGen(GraphicsDevice graphicsDevice, Effect textureEffect, Effect multiTextureEffect, Texture2D texture, Texture2D texture2, Texture2D blendMap, Texture2D normal, float texScale)
        {
            this.graphicsDevice = graphicsDevice;

            this.textureEffect = textureEffect;
            this.multiTextureEffect = multiTextureEffect;
            this.texture = texture;
            this.texture2 = texture2;
            this.blendMap = blendMap;
            this.normal = normal;

            GenTriGrid(100, 100, 1.0f, 1.0f, new Vector3(0.0f, 0.0f, 0.0f), verts, normals, UVs, indices, texScale);
        }

        public void GenTriGrid(int numVertRows,
        int numVertCols,
        float dx,
        float dz,
        Vector3 center,
        List<Vector3> verts,
        List<Vector3> normals,
        List<Vector2> UVs,
        List<short> indices,
        float texScale)
        {
            int numVertices = numVertRows * numVertCols;
            int numCellRows = numVertRows - 1;
            int numCellCols = numVertCols - 1;

            int numTris = numCellRows * numCellCols * 2;


            float width = (float)numCellCols * dx;
            float depth = (float)numCellRows * dz;

            // --------- Build Vertices --------- 
            for (uint i = 0; i < numVertices; i++)
            {
                verts.Add(Vector3.Zero);
                normals.Add(Vector3.Zero);
                UVs.Add(Vector2.Zero);
            }


            float xOffset = -width * 0.5f;
            float zOffset = depth * 0.5f;

            int k = 0;
            for (float i = 0; i < numVertRows; i++)
            {
                for (float j = 0; j < numVertCols; j++)
                {
                    // Negate the depth coordiante to put in
                    // quadrant four. Then offset to center about
                    // coordinate system.
                    Vector3 v = verts[k];
                    v.X = -2.0f;
                    v.Y = -i * dz + zOffset;
                    v.Z = j * dx + xOffset;

                    // Translate so that the center of the grid is at the 
                    // specified 'center' parameter
                    Matrix T = Matrix.CreateTranslation(center);
                    verts[k] = Vector3.Transform(v, T);
                    normals[k] = new Vector3(0.0f, 1.0f, 0.0f);
                    UVs[k] = new Vector2((float)j, (float)i) * texScale;

                    k++;
                }
            }
            VertexPNT[] vertsColors = new VertexPNT[verts.Count];
            for (int i = 0; i < verts.Count; i++)
            {
                vertsColors[i] = new VertexPNT(verts[i], normals[i], UVs[i]);
            }
            vertexBufferGrid = new VertexBuffer(graphicsDevice, typeof(VertexPNT), verts.Count, BufferUsage.WriteOnly);
            vertexBufferGrid.SetData<VertexPNT>(vertsColors);

            // --------- Build indices ---------
            for (uint i = 0; i < numTris * 3; i++)
                indices.Add(0);

            k = 0;
            for (short i = 0; i < (short)numCellRows; i++)
            {
                for (short j = 0; j < (short)numCellCols; j++)
                {
                    indices[k] = (short)(i * numVertCols + j);
                    indices[k + 1] = (short)(i * numVertCols + j + 1);
                    indices[k + 2] = (short)((i + 1) * numVertCols + j);

                    indices[k + 3] = (short)((i + 1) * numVertCols + j);
                    indices[k + 4] = (short)(i * numVertCols + j + 1);
                    indices[k + 5] = (short)((i + 1) * numVertCols + j + 1);

                    // next Quad
                    k += 6;
                }
            }

            indexBufferGrid = new IndexBuffer(graphicsDevice, typeof(short), indices.Count, BufferUsage.WriteOnly);
            indexBufferGrid.SetData<short>(indices.ToArray());

        }
    }
}
