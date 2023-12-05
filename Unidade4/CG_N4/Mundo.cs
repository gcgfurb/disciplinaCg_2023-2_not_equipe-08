#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

//FIXME: padrão Singleton

namespace gcgcg
{
    public class Mundo : GameWindow
    {
      Objeto mundo;
      private char rotuloNovo = '?';
      private Objeto objetoSelecionado = null;

      private Shader _shaderBranca;
      private Shader _shaderVermelha;
      private Shader _shaderVerde;
      private Shader _shaderAzul;
      private Shader _shaderCiano;
      private Shader _shaderMagenta;
      private Shader _shaderAmarela;
      private Shader _shaderTexture;
      private Camera _camera;
      private Cubo cubo;
      private Texture _texture;
      private bool _firstMove = true;
      private Vector2 _lastPos;

      public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
          : base(gameWindowSettings, nativeWindowSettings)
      {
        mundo = new Objeto(null, ref rotuloNovo);
      }

      protected override void OnLoad()
      {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        GL.Enable(EnableCap.DepthTest);

        #region Cores
        _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
        _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
        _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
        _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
        _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
        _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
        _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
        #endregion

        #region Objeto: ponto  
        objetoSelecionado = new Ponto(mundo, ref rotuloNovo, new Ponto4D(2.0, 0.0))
        {
          PrimitivaTipo = PrimitiveType.Points,
          PrimitivaTamanho = 5
        };
        #endregion

        #region Texturas
        _shaderTexture = new Shader("Shaders/shaderTexture.vert", "Shaders/shaderTexture.frag");
        _texture = Texture.LoadFromFile("Resources/Texture.jpeg");
        #endregion

        cubo = new Cubo(mundo, ref rotuloNovo);
        objetoSelecionado = cubo;
        objetoSelecionado.shaderCor = _shaderTexture;

        #region Camera
        _camera = new Camera(Vector3.UnitZ * 5, Size.X / (float)Size.Y);
        #endregion

        CursorState = CursorState.Grabbed;
      }

      protected override void OnRenderFrame(FrameEventArgs e)
      {
          base.OnRenderFrame(e);

          GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

          mundo.Desenhar(new Transformacao4D(), _camera);

          SwapBuffers();
      }

      protected override void OnUpdateFrame(FrameEventArgs e)
      {
        base.OnUpdateFrame(e);
        
        // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc   TODO: forma otimizada para teclado.
        #region Teclado
        var input = KeyboardState;
        if (input.IsKeyDown(Keys.Escape))
          Close();
        if (input.IsKeyPressed(Keys.Space))
        {
          if (objetoSelecionado == null)
            objetoSelecionado = mundo;
          objetoSelecionado.shaderCor = _shaderBranca;
          objetoSelecionado = mundo.GrafocenaBuscaProximo(objetoSelecionado);
          objetoSelecionado.shaderCor = _shaderAmarela;
          cubo.shaderCor = _shaderTexture;
        }
        if (input.IsKeyPressed(Keys.G))
          mundo.GrafocenaImprimir("");
        if (input.IsKeyPressed(Keys.P) && objetoSelecionado != null)
          System.Console.WriteLine(objetoSelecionado.ToString());
        if (input.IsKeyPressed(Keys.M) && objetoSelecionado != null)
          objetoSelecionado.MatrizImprimir();
        if (input.IsKeyPressed(Keys.I) && objetoSelecionado != null)
          objetoSelecionado.MatrizAtribuirIdentidade();
        if (input.IsKeyPressed(Keys.Left) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(-0.05, 0, 0);
        if (input.IsKeyPressed(Keys.Right) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(0.05, 0, 0);
        if (input.IsKeyPressed(Keys.Up) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(0, 0.05, 0);
        if (input.IsKeyPressed(Keys.Down) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(0, -0.05, 0);
        if (input.IsKeyPressed(Keys.O) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(0, 0, 0.05);
        if (input.IsKeyPressed(Keys.L) && objetoSelecionado != null)
          objetoSelecionado.MatrizTranslacaoXYZ(0, 0, -0.05);
        if (input.IsKeyPressed(Keys.PageUp) && objetoSelecionado != null)
          objetoSelecionado.MatrizEscalaXYZ(2, 2, 2);
        if (input.IsKeyPressed(Keys.PageDown) && objetoSelecionado != null)
          objetoSelecionado.MatrizEscalaXYZ(0.5, 0.5, 0.5);
        if (input.IsKeyPressed(Keys.Home) && objetoSelecionado != null)
          objetoSelecionado.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);
        if (input.IsKeyPressed(Keys.End) && objetoSelecionado != null)
          objetoSelecionado.MatrizEscalaXYZBBox(2, 2, 2);
        if (input.IsKeyPressed(Keys.M) && objetoSelecionado != null)
          objetoSelecionado.MatrizRotacao(10);
        if (input.IsKeyPressed(Keys.N) && objetoSelecionado != null)
          objetoSelecionado.MatrizRotacao(-10);
        if (input.IsKeyPressed(Keys.K) && objetoSelecionado != null)
          objetoSelecionado.MatrizRotacaoZBBox(10);
        if (input.IsKeyPressed(Keys.L) && objetoSelecionado != null)
          objetoSelecionado.MatrizRotacaoZBBox(-10);

        const float cameraSpeed = 1.5f;
        if (input.IsKeyDown(Keys.Z))
          _camera.Position = Vector3.UnitZ * 5;
        if (input.IsKeyDown(Keys.W))
          _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        if (input.IsKeyDown(Keys.S))
          _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        if (input.IsKeyDown(Keys.A))
          _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        if (input.IsKeyDown(Keys.D))
          _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        if (input.IsKeyDown(Keys.RightShift))
          _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        if (input.IsKeyDown(Keys.LeftShift))
          _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        
        #endregion

        #region  Mouse
        const float sensitivity = 0.1f;

        if (MouseState.IsButtonPressed(MouseButton.Left))
        {
        }
        if (MouseState.IsButtonDown(MouseButton.Right) && objetoSelecionado != null)
        {
          int janelaLargura = Size.X;
          int janelaAltura = Size.Y;
          Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
          Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

          objetoSelecionado.PontosAlterar(sruPonto, 0);
        }
        if (MouseState.IsButtonReleased(MouseButton.Right))
        {
        }
        var mouse = MouseState;

        if (_firstMove)
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity;
        }
        #endregion
        
        if (input.IsKeyDown(Keys.D0))
        {
          cubo.shaderCor = _shaderTexture;
        }
        if (input.IsKeyDown(Keys.D1))
        {}
        if (input.IsKeyDown(Keys.D2))
        {}
        if (input.IsKeyDown(Keys.D3))
        {}
        if (input.IsKeyDown(Keys.D4))
        {}
        if (input.IsKeyDown(Keys.D5))
        {}
        if (input.IsKeyDown(Keys.D6))
        {}
      }

      protected override void OnMouseWheel(MouseWheelEventArgs e)
      {
          base.OnMouseWheel(e);
          _camera.Fov -= e.OffsetY;
      }

      protected override void OnResize(ResizeEventArgs e)
      {
          base.OnResize(e);

          GL.Viewport(0, 0, Size.X, Size.Y);
          _camera.AspectRatio = Size.X / (float)Size.Y;
      }
    }
}