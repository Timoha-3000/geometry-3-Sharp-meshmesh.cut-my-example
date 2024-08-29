using System;
using g3;

namespace geometry_3_Sharp_meshmesh.cut_my_example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mesh1 = StandardMeshReader.ReadMesh("321.stl");
            var mesh2 = StandardMeshReader.ReadMesh("132.stl");

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
