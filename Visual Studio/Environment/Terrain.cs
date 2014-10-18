﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Wheat.Manager;

namespace Wheat.Environment
{
    class Terrain
    {
        #region Fields

        private VertexBuffer _vertexBuffer;

        private Model _mesh;
        private Effect _effect;

        EffectParameter _projectionParameter;
        EffectParameter _viewParameter;
        EffectParameter _worldParameter;
        EffectParameter _ambientIntensityParameter;
        EffectParameter _ambientColorParameter;

        #endregion

        #region Public Methods

        public Terrain(GraphicsDevice graphicsDevice)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);

            _vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            _vertexBuffer.SetData<VertexPositionColor>(vertices);
        }

        public void LoadContent(ContentManager content)
        {
            _mesh = content.Load<Model>("Models/Object");
            _effect = content.Load<Effect>("Effects/Basic");

            // Bind the parameters with the shader.
            _worldParameter = _effect.Parameters["World"];
            _viewParameter = _effect.Parameters["View"];
            _projectionParameter = _effect.Parameters["Projection"];

            _ambientColorParameter = _effect.Parameters["AmbientColor"];
            _ambientIntensityParameter = _effect.Parameters["AmbientIntensity"];
        }

        public void Draw(GraphicsDevice graphicsDevice, Camera camera)
        {
            _projectionParameter.SetValue(camera.ProjectionMatrix);
            _viewParameter.SetValue(camera.ViewMatrix);
            _worldParameter.SetValue(camera.WorldMatrix);
            
            _ambientIntensityParameter.SetValue(0.3f);
            _ambientColorParameter.SetValue(new Vector4(1, 1, 1, 1));
            
            graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _effect.CurrentTechnique = _effect.Techniques["Technique1"];
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }
        }

        #endregion
    }
}
