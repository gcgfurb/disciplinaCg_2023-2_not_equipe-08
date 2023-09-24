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
using System.Collections.Generic;
using OpenTK.Mathematics;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    Objeto mundo;
    private char rotuloAtual = '?';
    private Objeto objetoSelecionado = null;

    private readonly float[] _sruEixos =
    {
      -0.0f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.0f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.0f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
    };

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;

    private bool _firstMove = true;
    private Vector2 _lastPos;

    private Objeto circleObject = null;
    private int circlePoints = 72;
    private int angle = 0;
    private float radius = 0.5f;
    private int currentPoint = 0;
    private float movement = 0.05f;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo = new Objeto(null, ref rotuloAtual);
    }

    private void Diretivas()
    {
#if DEBUG
      Console.WriteLine("Debug version");
#endif      
#if RELEASE
    Console.WriteLine("Release version");
#endif      
#if CG_Gizmo      
      Console.WriteLine("#define CG_Gizmo  // debugar gráfico.");
#endif
#if CG_OpenGL      
      Console.WriteLine("#define CG_OpenGL // render OpenGL.");
#endif
#if CG_DirectX      
      Console.WriteLine("#define CG_DirectX // render DirectX.");
#endif
#if CG_Privado      
      Console.WriteLine("#define CG_Privado // código do professor.");
#endif
      Console.WriteLine("__________________________________ \n");
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      Diretivas();

      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);

      #region Eixos: SRU  
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
            #endregion

#if CG_Privado
      #region Objeto: circulo  
      objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.2, new Ponto4D());
      objetoSelecionado.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

      #region Objeto: SrPalito  
      objetoSelecionado = new SrPalito(mundo, ref rotuloAtual);
      #endregion

      #region Objeto: Spline
      objetoSelecionado = new Spline(mundo, ref rotuloAtual);
      #endregion
#endif

      angle = 360 / circlePoints;
      currentPoint = 45 / angle;

      #region Objeto: circulo  
      circleObject = new Circulo(null, ref rotuloAtual, radius, circlePoints);
      #endregion

      #region Objeto: segmento de reta  
      objetoSelecionado = new SegReta(mundo, ref rotuloAtual, new Ponto4D(0.0, 0.0), circleObject.PontosId(currentPoint));
      #endregion
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo      
      Sru3D();
#endif
      mundo.Desenhar();
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      #region Variables
      int nextAngle = 0;
      #endregion

      #region Teclado
      var input = KeyboardState;
      
      if (input.IsKeyDown(Keys.Escape))
      {
        Close();
      } else if (input.IsKeyPressed(Keys.Q))
      {
        // Left
        for (int i = 0; i < circlePoints; i++) {
          circleObject.PontosAlterar(new Ponto4D(circleObject.PontosId(i).X - movement, circleObject.PontosId(i).Y), i);
        }
        circleObject.ObjetoAtualizar();

        objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(0).X - movement, objetoSelecionado.PontosId(0).Y), 0);
        objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(1).X - movement, objetoSelecionado.PontosId(1).Y), 1);
        objetoSelecionado.ObjetoAtualizar();
      }  else if (input.IsKeyPressed(Keys.W)) {
        // Right
        for (int i = 0; i < circlePoints; i++) {
          circleObject.PontosAlterar(new Ponto4D(circleObject.PontosId(i).X + movement, circleObject.PontosId(i).Y), i);
        }
        circleObject.ObjetoAtualizar();

        objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(0).X + movement, objetoSelecionado.PontosId(0).Y), 0);
        objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(1).X + movement, objetoSelecionado.PontosId(1).Y), 1);
        objetoSelecionado.ObjetoAtualizar();
      } else if (input.IsKeyPressed(Keys.A)) {
        // Scale down
        radius -= 0.05f;

        for (int i = 0; i < circlePoints; i++) {
          Ponto4D pto = Matematica.GerarPtosCirculo(nextAngle, radius);
          circleObject.PontosAlterar(new Ponto4D(pto.X + objetoSelecionado.PontosId(0).X, pto.Y), i);
          circleObject.ObjetoAtualizar();
          nextAngle += angle;
        }

        objetoSelecionado.PontosAlterar(new Ponto4D(circleObject.PontosId(currentPoint)), 1);
        objetoSelecionado.ObjetoAtualizar();

      } else if (input.IsKeyPressed(Keys.S)) {
        // Scale up
        radius += 0.05f;

        for (int i = 0; i < circlePoints; i++) {
          Ponto4D pto = Matematica.GerarPtosCirculo(nextAngle, radius);
          circleObject.PontosAlterar(new Ponto4D(pto.X + objetoSelecionado.PontosId(0).X, pto.Y), i);
          circleObject.ObjetoAtualizar();
          nextAngle += angle;
        }

        objetoSelecionado.PontosAlterar(new Ponto4D(circleObject.PontosId(currentPoint)), 1);
        objetoSelecionado.ObjetoAtualizar();

      } else if (input.IsKeyPressed(Keys.Z)) {
        // Counter clockwise
        if (currentPoint == circlePoints-1) {
          currentPoint = 0;
        } else {
          currentPoint += 1;
        }
        objetoSelecionado.PontosAlterar(new Ponto4D(circleObject.PontosId(currentPoint)), 1);
        objetoSelecionado.ObjetoAtualizar();
      } else if (input.IsKeyPressed(Keys.X)) {
        // Clockwise
        if (currentPoint == 0) {
          currentPoint = circlePoints-1;
        } else {
          currentPoint -= 1;
        }
        objetoSelecionado.PontosAlterar(circleObject.PontosId(currentPoint), 1);
        objetoSelecionado.ObjetoAtualizar();
      }
      #endregion
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

      GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);

      base.OnUnload();
    }

#if CG_Gizmo
    private void Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
