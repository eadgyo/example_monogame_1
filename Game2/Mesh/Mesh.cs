using Game2.Vertex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game2
{
    public class Mesh
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Scale { get; set; }

        public Model Model { get; private set; }

        private Matrix[] modelTransforms;

        private BoundingSphere boundingSphere;

        public Material Material { get; set; }

        public Vector3 LightVec { get; set; }

        public float SpecularPower { get; set;}

        public Vector3 LightPos { get; set; }

        public BoundingSphere BoundingSphere
        {
            get
            {
                Matrix worldTransofrm = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransofrm);
                
                return transformed;
            }
        }

        private GraphicsDevice graphicsDevice;



        public Mesh(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            this.Model = Model;
            this.Material = new Material();

            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            BuildBoundingSphere();
            generateTags();

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.LightVec = Vector3.Normalize(new Vector3(0.5f, 1.0f, 0.0f));
            this.LightPos = new Vector3(10, 0, -100);
            SpecularPower = 2.50f;

            this.graphicsDevice = graphicsDevice;
        }

        public Mesh(Model model)
        {
            Model = model;
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            // Calculate the base transformtion by combining translation, rotation and scaling
            Matrix baseWorld = Matrix.CreateScale(Scale) + Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BasicEffect effect = (BasicEffect)meshPart.Effect;

                    effect.World = localWorld;
                    effect.View = View;
                    effect.Projection = Projection;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        private void BuildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }

        internal void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Calculate the base transformtion by combining translation, rotation and scaling
            Matrix baseWorld = Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    Material.SetEffectParameters(effect);
                    

                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        Matrix gWVP = localWorld * View * Projection;

                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", View);
                        setEffectParameter(effect, "Projection", Projection);
                        setEffectParameter(effect, "gEyePos", CameraPosition);
                        setEffectParameter(effect, "gWorldInverseTranspose", Matrix.Transpose(Matrix.Invert(localWorld)));
                        setEffectParameter(effect, "gSpecularPower", SpecularPower);
                        setEffectParameter(effect, "gWVP", gWVP);

                        // Diffuse
                        setEffectParameter(effect, "gLightVecW", LightVec);

                        // Spotlight
                        setEffectParameter(effect, "gLightPosW", this.LightPos);
                        setEffectParameter(effect, "gAttenuation012", new Vector3(1.0f, 0.00050f, 0.00005f));
                    }
                }
                mesh.Draw();
            }
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        Vector4 color = new Vector4(effect.DiffuseColor.X, effect.DiffuseColor.Y, effect.DiffuseColor.Z, 1.0f);
                        MeshTag tag = new MeshTag(color, effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }

                }
            }
        }
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
                }
            }
        }

        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (((MeshTag)part.Tag).CachedEffect != null)
                    {
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
                    }
                }
            }
        }

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    if (CopyEffect)
                        toSet = effect.Clone();

                    MeshTag tag = ((MeshTag)part.Tag);

                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                    {
                        setEffectParameter(toSet, "TextureEnabled", false);
                    }

                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "gDiffuseMtrl", tag.Color);
                    setEffectParameter(toSet, "gSpecularPower", tag.SpecularPower);

                    part.Effect = toSet;
                }
            }
        }

        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
            {
                Debug.WriteLine("Mesh::SetEffectParameter() -> Not found " + paramName);
                return;
            }

            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is Vector4)
                effect.Parameters[paramName].SetValue((Vector4)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
            else if (val is float)
                effect.Parameters[paramName].SetValue((float)val);
        }


    }
}
