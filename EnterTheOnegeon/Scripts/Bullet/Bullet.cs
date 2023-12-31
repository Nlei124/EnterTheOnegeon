﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    class Bullet : GameObject 
    {
        /// <summary> Things that were alreay hit by the bullet </summary>
        private List<GameObject> hitObjects;
        /// <summary> The bullet's velocity, but normalized </summary>
        private Vector2 trajectory;

        /// <summary> seconds taken to travel from player to trajectory</summary>
        private double speed;

        /// <summary> time the bullet has been alive </summary>
        private double timer;

        /// <summary> how many more enemies bullet can pass through </summary>
        private int passes;

        /// <summary> X coordinates of bullet's spawn location </summary>
        private int spawnX;

        /// <summary> the Y coordinates of bullet's spawn location </summary>
        private int spawnY;

        private int damage;

        //Creates inactive bullets
        public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            // Bullets are created with *zero* stats
            // When a bullet is reset via Reset() being called in BulletManager, it gets stats
            hitObjects = new List<GameObject>();
        }

        /// <summary>
        /// Property that returns true when the bullet can pass through more enemies
        /// Bullets despawn when they collide with any wall
        /// </summary>
        public override bool Active
        {
            get 
            {
                  return passes > 0 &&
                    // North wall
                    rectangle.Center.Y > 0 + 96*2 &&
                    // West wall
                    rectangle.Center.X > 0 + 96*2 &&
                    // East wall
                    rectangle.Center.Y < 2176 - 32*2 &&
                    // South wall
                    rectangle.Center.X < 3840 - 96*2;
            }
        }

        /// <summary>
        /// Whenever this  bullet hits an enemy decrement number of passes and make 
        /// the enemy take damage.
        /// </summary>
        /// <param name="enem"></param>
        /// <returns>true</returns>
        public void HitEnemy(Enemy enem)
        {
            passes--;
            hitObjects.Add(enem);
            enem.TakeDamage(damage);
        }

        public void HitPlayer(Player player)
        {
            passes--;
            hitObjects.Add(player);
            player.TakeDamage(damage);
        }

        public void Update(GameTime gameTime)
        {
            if (this.Active)
            {
                // Sets timer to amount of time bullet has been alive
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                /* Original
                rectangle.X = (int)(spawnX + (trajectory.X * 1000 * timer / speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * 1000 * timer / speed));
                */
                int arbConst = 100;
                actualX = (float)(spawnX + (trajectory.X * arbConst * timer * speed));
                actualY = (float)(spawnY + (trajectory.Y * arbConst * timer * speed));

                rectangle.X = (int)(spawnX + (trajectory.X * arbConst * timer * speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * arbConst * timer * speed));
            }
        }

        /// <summary>
        /// Draws this bullet if it is active
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if(this.Active)
            {
                base.Draw(sb);
            }
        }

        //Overriding so that when bullets are inactive they don't collide with anything
        //And if they hit an object it doesn't not register
        public override bool CollideWith(GameObject other)
        {
            if (!Active)
                return false;
            foreach(GameObject gob in hitObjects)
            {
                if (gob == other)
                    return false;
            }
            return base.CollideWith(other);
        }

        /// <summary>
        /// Resets(Spawns) an inactive bullet
        /// Use the center for where you spawn it
        /// </summary>
        /// <param name="spaX">Spawning x pos</param>
        /// <param name="spaY">Spawning y pos</param>
        /// <param name="posToMove">Where to go</param>
        /// <param name="bStats">The stats</param>
        public void Reset(int spaX, int spaY, Vector2 posToMove, BulletStats bStats)
        {
            hitObjects.Clear();
            rectangle = new Rectangle(spaX-bStats.Size/2, spaY- bStats.Size/2, bStats.Size, bStats.Size);
            spawnX = spaX - bStats.Size / 2;
            spawnY = spaY - bStats.Size / 2;
            actualX = spaX;
            actualY = spaY;
            trajectory = VectorToPosition(posToMove);
            trajectory.Normalize();
            timer = 0;
            speed = bStats.Speed;
            passes = bStats.Passes;
            damage = bStats.Damage;
        }

        /// <summary>
        /// Resets(Spawns) an inactive bullet
        /// Use the center for where you spawn it
        /// </summary>
        /// <param name="spaX">Spawning x pos</param>
        /// <param name="spaY">Spawning y pos</param>
        /// <param name="bStats">The stats</param>
        public void Reset(Texture2D textur, int spaX, int spaY, Vector2 traject, BulletStats bStats)
        {
            hitObjects.Clear();
            sprite = textur;
            rectangle = new Rectangle(spaX - bStats.Size / 2, spaY - bStats.Size / 2, bStats.Size, bStats.Size);
            spawnX = spaX - bStats.Size / 2;
            spawnY = spaY - bStats.Size / 2;
            actualX = spaX;
            actualY = spaY;
            trajectory = traject;
            trajectory.Normalize();
            timer = 0;
            speed = bStats.Speed;
            passes = bStats.Passes;
            damage = bStats.Damage;
        }
    }
}
