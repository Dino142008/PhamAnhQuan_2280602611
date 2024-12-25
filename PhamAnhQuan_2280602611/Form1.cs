using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using PhamAnhQuan_2280602611.model;

namespace PhamAnhQuan_2280602611
{
    public partial class frmSanpham : Form
    {
        public frmSanpham()
        {
            InitializeComponent();
        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            LoadLoaiSP();
            LoadSanpham();
        }

        private void LoadLoaiSP()
        {
            using (var dbContext = new Model1())
            {
                var loaiSPList = dbContext.LoaiSPs.ToList();
                cmbLoaiSP.DataSource = loaiSPList;
                cmbLoaiSP.DisplayMember = "TenLoai"; // Tên hiển thị
                cmbLoaiSP.ValueMember = "MaLoai"; // Giá trị được lưu
            }
        }

        private void LoadSanpham()
        {
            using (var dbContext = new Model1())
            {
                var sanphamList = dbContext.Sanphams.Include(s => s.LoaiSP).ToList(); // Bao gồm thông tin loại sản phẩm nếu cần
                dtgSanpham.DataSource = sanphamList.Select(sp => new
                {
                    sp.MaSP,
                    sp.TenSP,
                    Ngaynhap = sp.Ngaynhap.ToString("yyyy-MM-dd"),
                    sp.MaLoai,
                    TenLoai = sp.LoaiSP.TenLoai // Lấy tên loại sản phẩm
                }).ToList();
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Nút đã được nhấn!"); // Thông báo kiểm tra

                using (var dbContext = new Model1())
                {
                    // Lấy thông tin sản phẩm từ các trường nhập liệu
                    string maSP = txtMaSP.Text;
                    string tenSP = txtTenSP.Text;
                    DateTime ngaynhap = dtNgaynhap.Value;
                    string maLoai = cmbLoaiSP.SelectedValue.ToString();

                    // Tạo đối tượng sản phẩm mới
                    Sanpham newSanpham = new Sanpham
                    {
                        MaSP = maSP,
                        TenSP = tenSP,
                        Ngaynhap = ngaynhap,
                        MaLoai = maLoai
                    };

                    // Thêm sản phẩm vào DbSet và lưu thay đổi
                    dbContext.Sanphams.Add(newSanpham);
                    dbContext.SaveChanges();

                    // Cập nhật DataGridView
                    LoadSanpham();

                    MessageBox.Show("Thêm sản phẩm thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close(); // Đóng form
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {

        }
    }
}