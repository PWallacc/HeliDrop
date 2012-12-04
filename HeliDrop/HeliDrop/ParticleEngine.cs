using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeliDrop
{
    class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private Texture2D texture;
        private int effect;

        public int srcHorizon = 225;
        public float destDepth;
        public float velocityDirection = .3f;


        public ParticleEngine(Texture2D texture, Vector2 location, int selection)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle>();
            this.effect = selection;
            random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            switch (effect)
            {
                case 1:
                     particles.Add(GenerateExhaustParticle());
                    break;
                case 2:
                    break;
                default:
                    break;
            }
 
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateExhaustParticle()
        {
            Vector2 velocity = new Vector2(velocityDirection * (float)(random.Next(2, 6)), .3f * (float)(random.Next(-10, -2)));
            Vector2 position = EmitterLocation + velocity;
            float angle = 0;
            float angularVelocity = .045f;
            Color color = new Color(
                        256,
                        256,
                        256,
                        120);

            destDepth = (position.Y - srcHorizon) / (720 - srcHorizon);
            float size = 0.23f + (destDepth * 0.5f);
            int ttl = 40;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}