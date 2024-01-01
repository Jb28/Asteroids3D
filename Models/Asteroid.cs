using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids3D.Models
{
    internal class Asteroid
    {
        public Vector3 position;
        public Matrix worldView;
        public Model model;
        private float angle;
        private float movementSpeedX;
        private float movementSpeedY;
        private float movementSpeedZ;

        public Asteroid(Vector3 position, Matrix worldView, Model model, float angle, float movementSpeedX, float movementSpeedY, float movementSpeedZ)
        {
            this.position = position;
            this.worldView = worldView;
            this.model = model;
            this.angle = angle;
            this.movementSpeedX = movementSpeedX;
            this.movementSpeedY = movementSpeedY;
            this.movementSpeedZ = movementSpeedZ;
        }

        public void Update()
        {
            angle += 0.05f;
            worldView = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(position);
            position += new Vector3(movementSpeedX, movementSpeedY, movementSpeedZ);
        }

    }
}
