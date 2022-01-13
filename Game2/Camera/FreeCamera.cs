using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2
{
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }

        public float Pitch { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Target { get; set; }

        private Vector3 translation;

        public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += translation;
        }

        public override void Update()
        {
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            // Offset the positon and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }

        public Vector3 TransformVector(Vector3 vec)
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            return Vector3.Transform(vec, rotation); ;
        }
    }
}
