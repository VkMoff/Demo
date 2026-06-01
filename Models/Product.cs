using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Test3;

public partial class Product
{
    public int Id { get; set; }

    public string Article { get; set; } = null!;

    public string? Name { get; set; }

    public string? Measurement { get; set; }

    public decimal? Price { get; set; }

    public float? Discount { get; set; }

    public int? SupplierId { get; set; }

    public int? ProducerId { get; set; }

    public uint? Amount { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Producer? Producer { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public ImageSource Image
    {
        get
        {
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            if (ImagePath.IsWhiteSpace() || ImagePath is null)
            {
                bitmap.UriSource = new("D:\\Download\\Демонстрационный экзамен\\1 июня\\312\\Модуль 1\\import\\picture.png");
            }
            else
            {
                bitmap.UriSource = new("D:\\Download\\Демонстрационный экзамен\\1 июня\\312\\Модуль 1\\import\\" + ImagePath);
            }
            bitmap.DecodePixelWidth = 200;
            bitmap.EndInit();
            return bitmap;
        }
    }
    public decimal? OldPrice
    {
        get
        {
            if (Discount > 0)
            {
                return Price;
            }
            return null;
        }
    }
    public decimal? DiscountPrice
    {
        get
        {
            return Math.Round((Price ?? 0) * (decimal)(100 - Discount) / 100, 2);
        }
    }
    public string BackgroundColor
    {
        get
        {
            if (Discount > 17)
            {
                return "#FFDEAD";
            }
            else if (Amount <= 0)
            {
                return "#22AAFF";
            }
            return "#FFFFFF";
        }
    }
}
