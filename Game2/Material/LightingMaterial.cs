using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2
{
    public class LightingMaterial : Material
    {
        public Vector4 AmbientMtrl { get; set; }

        public Vector4 AmbientLight { get; set; }

        public Vector4 DiffuseMtrl { get; set; }

        public Vector4 DiffuseLight { get; set; }

        public Vector4 SpecularMtrl { get; set; }

        public Vector4 SpecularLight { get; set; }

        public float SpecularPower { get; set; }


        public LightingMaterial()
        {
            AmbientMtrl = new Vector4(0.3f, 0.2f, 0.3f, 1.0f);
            AmbientLight = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            DiffuseMtrl = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            DiffuseLight = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);


            SpecularMtrl = new Vector4(0.6f, 0.5f, 0.5f, 1.0f);
            SpecularLight = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            SpecularPower = 0.5f;
        }

        public override void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["gAmbientMtrl"] != null)
                effect.Parameters["gAmbientMtrl"].SetValue(AmbientMtrl);

            if (effect.Parameters["gAmbientLight"] != null)
                effect.Parameters["gAmbientLight"].SetValue(AmbientLight);

            if (effect.Parameters["gDiffuseMtrl"] != null)
                effect.Parameters["gDiffuseMtrl"].SetValue(DiffuseMtrl);

            if (effect.Parameters["gDiffuseLight"] != null)
                effect.Parameters["gDiffuseLight"].SetValue(DiffuseLight);

            if (effect.Parameters["gSpecularMtrl"] != null)
                effect.Parameters["gSpecularMtrl"].SetValue(SpecularMtrl);

            if (effect.Parameters["gSpecularLight"] != null)
                effect.Parameters["gSpecularLight"].SetValue(SpecularLight);

            if (effect.Parameters["gSpecularPower"] != null)
                effect.Parameters["gSpecularPower"].SetValue(SpecularPower);
        }
    }
}
