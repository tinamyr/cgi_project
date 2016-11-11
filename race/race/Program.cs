

#region --- Using Directives ---

using System;
using Engine.cgimin.camera;
using Engine.cgimin.material.simplereflection;
using Engine.cgimin.material.simpletexture;
using Engine.cgimin.material.wobble1;
using Engine.cgimin.material.wobble2;
using Engine.cgimin.object3d;
using Engine.cgimin.texture;
using Engine.cgimin.material.ambientdiffuse;
using Engine.cgimin.material.ambientdiffusespecular;
using Engine.cgimin.material.normalmapping;
using Engine.cgimin.light;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

#endregion --- Using Directives ---

namespace race
{

    public class CubeExample : GameWindow
    {

        // das Beispiel-Objekt
        private ObjLoaderObject3D exampleObject;


        // unsere textur-ID
        private int textureID;
        private int normalTextureID;

        // Materialien
        private SimpleReflectionMaterial simpleReflectionMaterial;
        private SimpleTextureMaterial simpleTextureMaterial;
        private Wobble1Material wobble1Material;
        private Wobble2Material wobble2Material;
        private AmbientDiffuseMaterial ambientDiffuseMaterial;
        private AmbientDiffuseSpecularMaterial ambientDiffuseSpecularMaterial;
        private NormalMappingMaterial normalMappingMaterial;

        private int updateCounter = 0;

        public CubeExample()
            : base(800, 600, new GraphicsMode(), "Face the Race", 0, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
        { }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Kamera initialisieren
            Camera.Init();
            Camera.SetWidthHeightFov(800, 600, 60);

            // Das Objekt laden
            exampleObject = new ObjLoaderObject3D("data/objects/torus_flat.obj", 1.0f, true);

            // Die Textur laden
            textureID = TextureManager.LoadTexture("data/textures/single_color.png");
            normalTextureID = TextureManager.LoadTexture("data/textures/brick_normal.png");


            // Materialien initialisieren
            simpleReflectionMaterial = new SimpleReflectionMaterial();
            simpleTextureMaterial = new SimpleTextureMaterial();
            wobble1Material = new Wobble1Material();
            wobble2Material = new Wobble2Material();
            ambientDiffuseMaterial = new AmbientDiffuseMaterial();
            ambientDiffuseSpecularMaterial = new AmbientDiffuseSpecularMaterial();
            normalMappingMaterial = new NormalMappingMaterial();

            // Tiefenpuffer einschalten
            GL.Enable(EnableCap.DepthTest);

            // Face-Culling
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            // Kameraposition setzen
            Camera.SetLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            // Licht setzen
            Light.SetDirectionalLight(new Vector3(1, 1, 1), new Vector4(0.5f, 0.5f, 0.5f, 1), new Vector4(1.0f, 1.0f, 1.0f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[OpenTK.Input.Key.Escape])
                this.Exit();

            if (Keyboard[OpenTK.Input.Key.F11])
                if (WindowState != WindowState.Fullscreen)
                    WindowState = WindowState.Fullscreen;
                else
                    WindowState = WindowState.Normal;

            // updateCounter ist für den Animationsfortschritt zuständig
            updateCounter++;


            Camera.updateFlyCamera(Keyboard[OpenTK.Input.Key.Left], Keyboard[OpenTK.Input.Key.Right], Keyboard[OpenTK.Input.Key.Up], Keyboard[OpenTK.Input.Key.Down]);

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Der Screen und er Tiefenpuffer (z-Buffer) wird gelöscht
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            exampleObject.Transformation = Matrix4.CreateRotationX(updateCounter / 150.0f);
            exampleObject.Transformation *= Matrix4.CreateRotationY(updateCounter / 110.0f);

            // Objekt wird gezeichnet
            normalMappingMaterial.Draw(exampleObject, textureID, normalTextureID, 25.0f);


            SwapBuffers();
        }



        protected override void OnUnload(EventArgs e)
        {
            exampleObject.UnLoad();
        }


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            Camera.SetWidthHeightFov(Width, Height, 60);
        }


        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Race-Project");
            using (CubeExample example = new CubeExample())
            {
                example.Run(60.0, 0.0);
            }
        }


    }
}

