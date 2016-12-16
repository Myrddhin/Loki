using System;
using System.Text;

namespace Loki.Common
{
    public class RandomGenerator
    {
        public DateTime SeedDate { get; set; }

        public int Seed { get; set; }

        public string SeedString { get; set; }

        private const string DefaultStringSeed = "#TEST";

        private Random rand;

        public RandomGenerator()
        {
            SeedDate = new DateTime(2100, 01, 01);
            rand = new Random();
            SeedString = DefaultStringSeed;
        }

        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public string GetString(int size)
        {
            var defaultLength = Encoding.Default.GetByteCount("a");
            var builder = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                byte[] buffer = new byte[defaultLength];
                rand.NextBytes(buffer);

                builder.Append(Encoding.Default.GetString(buffer));
            }

            return builder.ToString();
        }

        public int GetInteger()
        {
            return rand.Next(10000);
        }

        public int? GetNullableInteger()
        {
            int? buffer = GetInteger();
            return rand.Next() % 4 == 0 ? null : buffer;
        }

        public DateTime? GetNullableDateAndTime()
        {
            return rand.Next() % 4 == 0 ? null : (DateTime?)GetDateAndTime();
        }

        public DateTime GetDateAndTime()
        {
            DateTime buffer = SeedDate.AddDays(GetInteger());
            return buffer.AddSeconds(GetInteger());
        }

        public T GetEnum<T>()
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(rand.Next(enumValues.Length - 1));
        }

        public double GetDouble()
        {
            return rand.NextDouble() * GetInteger();
        }

        public double GetDouble(double max)
        {
            return rand.NextDouble() * rand.Next(Convert.ToInt32(Math.Ceiling(max)));
        }

        public double? GetNullableDouble()
        {
            double? buffer = GetDouble();
            return rand.Next() % 4 == 0 ? null : buffer;
        }

        public bool GetBoolean()
        {
            return rand.Next() % 2 == 0;
        }
    }
}