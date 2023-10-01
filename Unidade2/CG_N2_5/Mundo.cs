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
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
    };

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;

    private bool _firstMove = true;
    private Vector2 _lastPos;

    private Objeto centerPoint = null;
    private int circlePoints = 72;
    private Objeto largeCircle = null;
    private float largeCircleRadius = 0.3f;
    private Objeto smallCircle = null;
    private float smallCircleRadius = 0.1f;
    private Objeto rectangle = null;
    private Ponto4D pointAt45Deg = null;
    private Ponto4D pointAt225Deg = null;

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

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

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

      centerPoint = new Ponto(mundo, ref rotuloAtual, new Ponto4D(0.3, 0.3))
      {
            PrimitivaTamanho = 10
      };

      largeCircle = new Circulo(mundo, ref rotuloAtual, largeCircleRadius, circlePoints, new Ponto4D(centerPoint.PontosId(0)))
      {
            PrimitivaTipo = PrimitiveType.LineLoop
      };

      smallCircle = new Circulo(centerPoint, ref rotuloAtual, smallCircleRadius, circlePoints, new Ponto4D(centerPoint.PontosId(0)))
      {
            PrimitivaTipo = PrimitiveType.LineLoop
      };

      int indexOf45Deg = 45 / (360 / circlePoints);
      int indexOf225Deg = 225 / (360 / circlePoints);

      pointAt45Deg = new Ponto4D(largeCircle.PontosId(indexOf45Deg));
      pointAt225Deg = new Ponto4D(largeCircle.PontosId(indexOf225Deg));

      rectangle = new Retangulo(largeCircle, ref rotuloAtual, new Ponto4D(pointAt45Deg), new Ponto4D(pointAt225Deg))
      {
            PrimitivaTipo = PrimitiveType.LineLoop
      };
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

      #region Variaveis
            double movement = 0.01;
      #endregion

      #region Teclado
      var input = KeyboardState;
      
      if (input.IsKeyDown(Keys.Escape))
      {
        Close();
      } else if (input.IsKeyPressed(Keys.C)) 
      {
            if (IsInsideOfCircle() || centerPoint.PontosId(0).Y + movement < largeCircleRadius)
            {
                  // Move UP
                  centerPoint.PontosAlterar(new Ponto4D(centerPoint.PontosId(0).X, centerPoint.PontosId(0).Y + movement), 0);
                  centerPoint.ObjetoAtualizar();
                  
                  for (int i = 0; i < circlePoints; i++) {
                        smallCircle.PontosAlterar(new Ponto4D(smallCircle.PontosId(i).X, smallCircle.PontosId(i).Y + movement), i);
                  }
                  smallCircle.ObjetoAtualizar();
            }
      } else if (input.IsKeyPressed(Keys.B)) 
      {
            if (IsInsideOfCircle() || (centerPoint.PontosId(0).Y - movement > largeCircleRadius))
            {
                  // Move DOWN
                  centerPoint.PontosAlterar(new Ponto4D(centerPoint.PontosId(0).X, centerPoint.PontosId(0).Y - movement), 0);
                  centerPoint.ObjetoAtualizar();
                  
                  for (int i = 0; i < circlePoints; i++) {
                        smallCircle.PontosAlterar(new Ponto4D(smallCircle.PontosId(i).X, smallCircle.PontosId(i).Y - movement), i);
                  }
                  smallCircle.ObjetoAtualizar();
            }
      } else if (input.IsKeyPressed(Keys.E)) 
      {
            if (IsInsideOfCircle() || (centerPoint.PontosId(0).X - movement > largeCircleRadius))
            {
                  // Move LEFT
                  centerPoint.PontosAlterar(new Ponto4D(centerPoint.PontosId(0).X - movement, centerPoint.PontosId(0).Y), 0);
                  centerPoint.ObjetoAtualizar();

                  for (int i = 0; i < circlePoints; i++) {
                        
                        smallCircle.PontosAlterar(new Ponto4D(smallCircle.PontosId(i).X - movement, smallCircle.PontosId(i).Y), i);
                  }
                  smallCircle.ObjetoAtualizar();
            }
      } else if (input.IsKeyPressed(Keys.D)) 
      {
            if (IsInsideOfCircle() || (centerPoint.PontosId(0).X + movement < largeCircleRadius))
            {
                  // Move RIGHT
                  centerPoint.PontosAlterar(new Ponto4D(centerPoint.PontosId(0).X + movement, centerPoint.PontosId(0).Y), 0);
                  centerPoint.ObjetoAtualizar();

                  for (int i = 0; i < circlePoints; i++) {
                        smallCircle.PontosAlterar(new Ponto4D(smallCircle.PontosId(i).X + movement, smallCircle.PontosId(i).Y), i);
                  }
                  smallCircle.ObjetoAtualizar();
            }
      }

      IsInsideOfRectangle();

      #endregion
    }

    protected void IsInsideOfRectangle() 
    {
      if (smallCircle.PontosId(0).X - smallCircleRadius < rectangle.PontosId(1).X 
            || smallCircle.PontosId(0).X - smallCircleRadius > rectangle.PontosId(0).X
            || smallCircle.PontosId(0).Y < rectangle.PontosId(2).Y
            || smallCircle.PontosId(0).Y > rectangle.PontosId(0).Y
      )
      {
            rectangle.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      } else 
      {
            rectangle.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      }
    }

    protected bool IsInsideOfCircle() 
    {
      double distance = Matematica.distancia(
                                                new Ponto4D(smallCircleRadius - smallCircle.PontosId(0).X, smallCircle.PontosId(0).Y), 
                                                new Ponto4D(largeCircleRadius - largeCircle.PontosId(0).X, largeCircle.PontosId(0).Y)
                                          );

      if (Math.Round(distance, 2) == Math.Round(largeCircleRadius, 2)) {
            return false;
      }
      return true;
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
