using System;
using g3;

namespace geometry_3_Sharp_meshmesh.cut_my_example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем базовый меш (коробка)
            DMesh3 boxMesh = new DMesh3();
            var x = new Vector3d(1, 1, 1);
            var y = new Vector3d(1, 1, 1);
            var z = new Vector3d(1, 1, 1);
            TrivialBox3Generator boxGen = new TrivialBox3Generator()
            {
                //Box = new Box3d(Vector3d.Zero, x, y, z, Vector3d.Zero),
            };
            boxMesh = boxGen.Generate().MakeDMesh();

            // Создаем второй меш (сфера)
            DMesh3 sphereMesh = new DMesh3();
            Sphere3Generator_NormalizedCube sphereGen = new Sphere3Generator_NormalizedCube()
            {
                Radius = 0.7f,
                //Slices = 16,
                
            };
            sphereMesh = sphereGen.Generate().MakeDMesh();

            // Перемещаем сферу, чтобы она пересекалась с коробкой
            //MeshTransforms.Translate(sphereMesh, new Vector3d(1.5, 0.5, 0.5));

            // Устанавливаем параметры для обрезки мешей
            MeshBoolean cutter = new MeshBoolean();
            cutter.Target = sphereMesh;
            cutter.Tool = boxMesh;
            cutter.Compute();
            //cutter.RemoveContained();


            // Выполняем операцию обрезки
            var success = cutter.Result;
            {
                // Результирующий меш после обрезки

                // Сохраняем результирующий меш в файл или визуализируем
                StandardMeshWriter.WriteMesh("cut_result.stl", success, WriteOptions.Defaults);
                StandardMeshWriter.WriteMesh("boxGen.stl", boxMesh, WriteOptions.Defaults);
                StandardMeshWriter.WriteMesh("sphereGen.stl", sphereMesh, WriteOptions.Defaults);

                
                Console.WriteLine("Обрезка успешно выполнена!");
            }
            {
                Console.WriteLine("Обрезка не удалась!");
            }
        }
    }
}
