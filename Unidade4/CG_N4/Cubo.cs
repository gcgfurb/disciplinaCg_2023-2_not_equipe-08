//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Ponto4D[] vertices;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;
    public readonly float[] _vertices =
    {
      // Positions          Normals              Texture coords
      // Back face
      -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
       1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
       1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
       1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
      -1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
      -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

      // Front face
      -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
       1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
       1.0f,  1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
       1.0f,  1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
      -1.0f,  1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
      -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

      // Left face
      -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
      -1.0f,  1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
      -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
      -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
      -1.0f, -1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
      -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

      // Right face
       1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
       1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
       1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
       1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
       1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
       1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

      // Bottom face
      -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
       1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
       1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
       1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
      -1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
      -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

      // Top face
      -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
       1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
       1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
       1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
      -1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
      -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
    };

    public Cubo(Objeto paiRef, ref char _rotulo) : base(paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.TriangleFan;
      PrimitivaTamanho = 10;

      vertices = new Ponto4D[]
      {
        new Ponto4D(-1.0f, -1.0f, -1.0f), //0
        new Ponto4D(-1.0f, -1.0f,  1.0f), //1
        new Ponto4D(-1.0f,  1.0f,  1.0f), //2
        new Ponto4D( 1.0f,  1.0f,  1.0f), //3
        new Ponto4D( 1.0f,  1.0f, -1.0f), //4
        new Ponto4D( 1.0f, -1.0f, -1.0f), //5
        new Ponto4D(-1.0f,  1.0f, -1.0f), //6
        new Ponto4D( 1.0f, -1.0f,  1.0f), //7
      };

      var indices = new uint[]
      {
        2, 6, 0, 0, 1, 2, // Left face
        1, 7, 3, 3, 2, 1, // Front face
        0, 5, 4, 4, 6, 0, // Back face
        6, 4, 3, 3, 2, 6, // Top face
        3, 4, 5, 5, 7, 3, // Right face
        0, 5, 7, 7, 1, 0, // Bottom face
      };

      foreach (int index in indices)
      {
        PontosAdicionar(vertices[index]);
      }

      var textureVertices = new Ponto4D[]
      {
        new Ponto4D(1.0f, 1.0f),
        new Ponto4D(1.0f, 0.0f),
        new Ponto4D(0.0f, 1.0f),
        new Ponto4D(0.0f, 0.0f),
      };

      var textureIndices = new uint[]
      {
        1, 0, 2, 2, 3, 1,
        3, 1, 0, 0, 2, 3,
        1, 0, 2, 2, 3, 1,
        1, 0, 2, 2, 3, 1,
        2, 0, 1, 1, 3, 2,
        2, 0, 1, 1, 3, 2,
      };

      foreach (int index in textureIndices)
      {
        texturePoints.Add(textureVertices[index]);
      }

      var normalVertices = new Ponto4D[]
      {
        new Ponto4D(0, 0, 1),
        new Ponto4D(1, 0, 0),
        new Ponto4D(0, 0, -1),
        new Ponto4D(-1, 0, 0),
        new Ponto4D(0, -1, 0),
        new Ponto4D(0, 1, 0),
      };

      var normalIndices = new int[]
      {
        2, 2, 2, 2, 2, 2, // Front face
        0, 0, 0, 0, 0, 0, // Back face
        1, 1, 1, 1, 1, 1, // Right face
        3, 3, 3, 3, 3, 3, // Left face
        4, 4, 4, 4, 4, 4, // Bottom face
        5, 5, 5, 5, 5, 5, // Top face
      };

      foreach (int index in normalIndices)
      {
        normalPoints.Add(normalVertices[index]);
      }

      PrimitivaTipo = PrimitiveType.Triangles;

      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif
  }
}
