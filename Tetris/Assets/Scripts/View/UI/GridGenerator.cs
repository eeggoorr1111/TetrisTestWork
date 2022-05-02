using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.View
{
    public sealed class GridGenerator
    {
        private static readonly float _offsetZ = 0.001f;
        private static readonly float _halfSizeCell = 0.5f;

        public GridGenerator(float widthLinesArg)
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uvs = new List<Vector2>();
            _halfWidthLines = widthLinesArg / 2;
        }


        private readonly List<Vector3> _vertices;
        private readonly List<int> _triangles;
        private readonly List<Vector2> _uvs;
        private readonly float _halfWidthLines;


        public Mesh Generate(Vector2Int maxCellArg)
        {
            Mesh mesh = new Mesh();

            _vertices.Clear();
            _triangles.Clear();
            _uvs.Clear();

            for (int i = 0; i < maxCellArg.y; i++)
                CreateRow(i, maxCellArg.x, -_offsetZ);

            for (int i = 0; i < maxCellArg.x; i++)
                CreateColumn(i, maxCellArg.y, _offsetZ);

            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.SetUVs(0, _uvs);

            return mesh;
        }


        private void CreateRow(int rowArg, int countColumdsArg, float zArg)
        {
            float yPos = rowArg + _halfSizeCell;
            Vector3 startLine = new Vector3(-_halfSizeCell, yPos, zArg);
            Vector3 endLine = new Vector3(countColumdsArg + _halfSizeCell, yPos, zArg);

            CreateLine(startLine, endLine, new Vector3(0, _halfWidthLines, 0));
        }
        private void CreateColumn(int columnArg, int countRowsArg, float zArg)
        {
            float xPos = columnArg + _halfSizeCell;
            Vector3 startLine = new Vector3(xPos, countRowsArg + _halfSizeCell, zArg);
            Vector3 endLine = new Vector3(xPos, -_halfSizeCell, zArg);

            CreateLine(startLine, endLine, new Vector3(_halfWidthLines, 0, 0));
        }
        private void CreateLine(Vector3 startArg, Vector3 endArg, Vector3 vertOffsetArg)
        {
            int startIndexVert = _vertices.Count;
            Vector3 vert1 = startArg + vertOffsetArg;
            Vector3 vert2 = endArg + vertOffsetArg;
            Vector3 vert3 = endArg - vertOffsetArg;
            Vector3 vert4 = startArg - vertOffsetArg;

            _vertices.Add(vert1);
            _vertices.Add(vert2);
            _vertices.Add(vert3);
            _vertices.Add(vert4);

            _uvs.Add(new Vector2(0, 1));
            _uvs.Add(new Vector2(1, 1));
            _uvs.Add(new Vector2(1, 0));
            _uvs.Add(new Vector2(0, 0));

            _triangles.Add(startIndexVert);
            _triangles.Add(startIndexVert + 1);
            _triangles.Add(startIndexVert + 2);

            _triangles.Add(startIndexVert);
            _triangles.Add(startIndexVert + 2);
            _triangles.Add(startIndexVert + 3);
        }
    }

}
