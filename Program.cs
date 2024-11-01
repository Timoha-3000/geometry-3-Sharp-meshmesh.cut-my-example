using System;
using System.Collections.Generic;
using System.IO;
using g3;

namespace geometry_3_Sharp_meshmesh.cut_my_example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mesh1 = StandardMeshReader.ReadMesh("sup2.stl");
            var mesh2 = StandardMeshReader.ReadMesh("5.stl");

            /*MeshExtrudeMesh extruder = new MeshExtrudeMesh(mesh1);
            //extruder.DefaultOffsetDistance = 0.01; // Минимальная толщина
            extruder.Extrude();*/


            // Создаем новую сетку для объемного треугольника
            DMesh3 thickMesh = new DMesh3();
            double offsetDistance = 0.01;

            // Создаем список для исходных и смещенных вершин
            List<int> originalVertices = new List<int>();
            List<int> offsetVertices = new List<int>();

            // Копируем оригинальные вершины и добавляем их в thickMesh
            foreach (int vid in mesh1.VertexIndices())
            {
                Vector3d originalVertex = mesh1.GetVertex(vid);
                int vID = thickMesh.AppendVertex(originalVertex);
                originalVertices.Add(vID);

                // Добавляем смещенную вершину
                Vector3d offsetVertex = originalVertex + new Vector3d(0, 0, offsetDistance);
                int offsetID = thickMesh.AppendVertex(offsetVertex);
                offsetVertices.Add(offsetID);
            }

            // Добавляем исходные и смещенные треугольники в thickMesh
            foreach (int tid in mesh1.TriangleIndices())
            {
                Index3i tri = mesh1.GetTriangle(tid);
                int v0 = originalVertices[tri.a];
                int v1 = originalVertices[tri.b];
                int v2 = originalVertices[tri.c];

                int v0_offset = offsetVertices[tri.a];
                int v1_offset = offsetVertices[tri.b];
                int v2_offset = offsetVertices[tri.c];

                // Верхний и нижний треугольники
                thickMesh.AppendTriangle(v0, v1, v2);
                thickMesh.AppendTriangle(v0_offset, v1_offset, v2_offset);

                // Боковые грани
                thickMesh.AppendTriangle(v0, v1, v1_offset);
                thickMesh.AppendTriangle(v0, v1_offset, v0_offset);

                thickMesh.AppendTriangle(v1, v2, v2_offset);
                thickMesh.AppendTriangle(v1, v2_offset, v1_offset);

                thickMesh.AppendTriangle(v2, v0, v0_offset);
                thickMesh.AppendTriangle(v2, v0_offset, v2_offset);
            }

            // Теперь thickMesh является объемной версией треугольника и готов для булевых операций
            Console.WriteLine("STL успешно загружен и преобразован в объемный треугольник для булевых операций.");
            StandardMeshWriter.WriteMesh("offset_result.stl", thickMesh, WriteOptions.Defaults);
            // Устанавливаем параметры для обрезки мешей
            MeshMeshCut cutter = new MeshMeshCut();
            cutter.Target = mesh1;
            cutter.CutMesh = mesh2;
            cutter.Compute();
            cutter.RemoveContained();


            // Выполняем операцию обрезки
            var success = cutter.Target;
            {
                // Результирующий меш после обрезки

                // Сохраняем результирующий меш в файл или визуализируем
                StandardMeshWriter.WriteMesh("cut_result.stl", success, WriteOptions.Defaults);
                //StandardMeshWriter.WriteMesh("boxGen.stl", boxMesh, WriteOptions.Defaults);
                //StandardMeshWriter.WriteMesh("sphereGen.stl", sphereMesh, WriteOptions.Defaults);

                
                Console.WriteLine("Обрезка успешно выполнена!");
            }
            {
                Console.WriteLine("Обрезка не удалась!");
            }
        }
    }
}
