using HeThongDatThucAn20.Data;

namespace HeThongDatThucAn20.ViewModels
{
    public class CartItem
    {
        HeThongDatDoAnContext db = new HeThongDatDoAnContext();
        public int MaHH { get; set; }
        public string? Hinh { get; set; }
        public string? TenHH { get; set; }
        public float DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * DonGia;
    }
}
