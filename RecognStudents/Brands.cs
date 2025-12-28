using System;

namespace NeuralNetwork1
{
    public enum BrandType : byte
    {
        Citroen = 0,
        Chevrolet = 1,
        Dodge = 2,
        Hyundai = 3,
        Lada = 4,
        Mitsubishi = 5,
        Maserati = 6,
        Opel = 7,
        Renault = 8,
        Tesla = 9,
        Undef = 255
    }

    public static class BrandMap
    {
        public static int FromFileName(string fileNameNoExt)
        {
            string s = fileNameNoExt.ToUpperInvariant();

            if (s.StartsWith("CH")) return (int)BrandType.Chevrolet;
            if (s.StartsWith("MZ")) return (int)BrandType.Maserati;

            if (s.StartsWith("C")) return (int)BrandType.Citroen;
            if (s.StartsWith("D")) return (int)BrandType.Dodge;
            if (s.StartsWith("H")) return (int)BrandType.Hyundai;
            if (s.StartsWith("L")) return (int)BrandType.Lada;
            if (s.StartsWith("M")) return (int)BrandType.Mitsubishi;
            if (s.StartsWith("O")) return (int)BrandType.Opel;
            if (s.StartsWith("R")) return (int)BrandType.Renault;
            if (s.StartsWith("T")) return (int)BrandType.Tesla;

            throw new Exception("Не смог определить класс по имени файла: " + fileNameNoExt);
        }
    }
}
