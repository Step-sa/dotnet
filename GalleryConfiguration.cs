using System;
using System.IO;
using System.Text.Json;

namespace ArtGalleryApp
{
    internal class GalleryException : Exception
    {
        public GalleryException(string message) : base(message)
        {
        }
    }

    internal class GalleryConfiguration
    {
        public decimal? StandardPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? MaxGroupSize { get; set; }

        public static GalleryConfiguration Load(string path)
        {
            string json = File.ReadAllText(path);
            GalleryConfiguration? settings = JsonSerializer.Deserialize<GalleryConfiguration>(json);
            if (settings == null ||
                settings.StandardPrice == null || settings.DiscountPrice == null ||
                settings.MaxGroupSize == null)
                throw new GalleryException("В конфигурации должны быть StandardPrice, DiscountPrice и MaxGroupSize.");

            if (settings.StandardPrice < 0 || settings.DiscountPrice < 0 ||
                settings.DiscountPrice > settings.StandardPrice || settings.MaxGroupSize <= 0)
                throw new GalleryException("Цены должны быть неотрицательными, льготная цена не выше обычной, размер группы больше нуля.");

            return settings;
        }
    }
}
