using System;
using System.Collections.Generic;
using g3;

namespace geometry_3_Sharp_meshmesh.cut_my_example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var mesh1 = StandardMeshReader.ReadMesh("321.stl");
            //var mesh2 = StandardMeshReader.ReadMesh("132.stl");

            // Вершины куба размером 1x1x1
            Vector3d[] vertices = new Vector3d[]
            {
                new Vector3d(0, 0, 0), // 0
                new Vector3d(300, 0, 0), // 1
                new Vector3d(300, 300, 0), // 2
                new Vector3d(0, 300, 0), // 3
                new Vector3d(0, 0, 40), // 4
                new Vector3d(300, 0, 40), // 5
                new Vector3d(300, 300, 40), // 6
                new Vector3d(0, 300, 40)  // 7
            };

            // Список треугольников куба
            List<Triangle3d> triangles = new List<Triangle3d>()
            {
                // Нижняя грань (0, 1, 2, 3)
                new Triangle3d(vertices[0], vertices[1], vertices[2]),
                new Triangle3d(vertices[0], vertices[2], vertices[3]),

                // Верхняя грань (4, 5, 6, 7)
                new Triangle3d(vertices[4], vertices[5], vertices[6]),
                new Triangle3d(vertices[4], vertices[6], vertices[7]),

                // Передняя грань (0, 1, 5, 4)
                new Triangle3d(vertices[0], vertices[1], vertices[5]),
                new Triangle3d(vertices[0], vertices[5], vertices[4]),

                // Задняя грань (3, 2, 6, 7)
                new Triangle3d(vertices[3], vertices[2], vertices[6]),
                new Triangle3d(vertices[3], vertices[6], vertices[7]),

                // Левая грань (0, 3, 7, 4)
                new Triangle3d(vertices[0], vertices[3], vertices[7]),
                new Triangle3d(vertices[0], vertices[7], vertices[4]),

                // Правая грань (1, 2, 6, 5)
                new Triangle3d(vertices[1], vertices[2], vertices[6]),
                new Triangle3d(vertices[1], vertices[6], vertices[5])
            };

            // Создаем пустой меш
            DMesh3 mesh1 = new DMesh3();

            // Хранение индексов вершин, чтобы правильно создавать треугольники
            Dictionary<Vector3d, int> vertexMap = new Dictionary<Vector3d, int>();

            // Проходим по каждому треугольнику и добавляем его в меш
            foreach (var tri in triangles)
            {
                int v0 = AddVertexIfNotExists(mesh1, vertexMap, tri.V0);
                int v1 = AddVertexIfNotExists(mesh1, vertexMap, tri.V1);
                int v2 = AddVertexIfNotExists(mesh1, vertexMap, tri.V2);

                mesh1.AppendTriangle(v0, v1, v2);
            }

            MeshTransforms.Translate(mesh1, new Vector3d(-150, -150, 10));
            var sphere = new Sphere3Generator_NormalizedCube();

            //var mesh2 = sphere.Generate().MakeDMesh();
            var mesh2 = StandardMeshReader.ReadMesh("5.stl");

            // Устанавливаем параметры для обрезки мешей
            MeshMeshCut cutter = new MeshMeshCut();
            cutter.Target = mesh1;
            cutter.CutMesh = mesh2;
            cutter.Compute();
            cutter.RemoveContained();

            // Выполняем операцию обрезки
            var success = cutter.Target;

            StandardMeshWriter.WriteMesh("cut_result.stl", success, WriteOptions.Defaults);
            StandardMeshWriter.WriteMesh("cutTool.stl", mesh1, WriteOptions.Defaults);
        }

        // Функция добавления вершины, если она еще не добавлена
        static int AddVertexIfNotExists(DMesh3 mesh, Dictionary<Vector3d, int> vertexMap, Vector3d vertex)
        {
            if (vertexMap.ContainsKey(vertex))
            {
                return vertexMap[vertex];
            }
            else
            {
                int index = mesh.AppendVertex(vertex);
                vertexMap[vertex] = index;
                return index;
            }
        }
    }
}
