using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De01.Models;
using System.Data.Entity; // Đối với Entity Framework 6


namespace De01
{
    public partial class frmSinhVien : Form

    {
        
        public frmSinhVien()
        {
            InitializeComponent();
        }
      

        private void button5_Click(object sender, EventArgs e)
        {
            // Xử lý lưu dữ liệu vào cơ sở dữ liệu khi nhấn Lưu
            string maSV = txtMaSV.Text;
            string hotenSV = txtHotenSV.Text;
            DateTime ngaySinh = dtpNgaySinh.Value;
            string maLop = cbLop.SelectedValue.ToString();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hotenSV))
            {
                MessageBox.Show("Mã sinh viên và họ tên không được để trống.");
                return;
            }

            using (var context = new Model1())
            {
                var newSV = new Sinhvien
                {
                    MaSV = maSV,
                    HotenSV = hotenSV,
                    NgaySinh = ngaySinh,
                    MaLop = maLop
                };

                context.Sinhviens.Add(newSV);
                context.SaveChanges();  // Lưu dữ liệu vào cơ sở dữ liệu
            }

            LoadData();  // Làm mới dữ liệu trong DataGridView
            MessageBox.Show("Thêm sinh viên thành công!");
            ClearFields();  // Xóa các trường dữ liệu sau khi thêm

            // Tắt nút Lưu và Không Lưu sau khi hoàn thành lưu
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }
        private void LoadData()
        {
            using (var context = new Model1())
            {
                // Load danh sách sinh viên
                var sinhvienList = context.Sinhviens.Include(sv => sv.Lop).ToList();
                dgvSinhvien.DataSource = sinhvienList.Select(sv => new
                {
                    sv.MaSV,
                    sv.HotenSV,
                    sv.NgaySinh,
                    Lop = sv.Lop.TenLop
                }).ToList();
                dgvSinhvien.Columns["NgaySinh"].DefaultCellStyle.Format = "yyyy-MM-dd";  // Định dạng cột Ngày Sinh

                // Load danh sách lớp vào ComboBox
                cbLop.DataSource = context.Lops.ToList();
                cbLop.DisplayMember = "TenLop";
                cbLop.ValueMember = "MaLop";
            }
        }

        private void ClearFields()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            cbLop.SelectedIndex = 0;
            txtMaSV.Enabled = true;  // Cho phép nhập Mã SV
        }
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSinhVien_FormClosing);


            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;

            // Đảm bảo DateTimePicker chỉ hiển thị ngày tháng năm
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "yyyy-MM-dd";
        }
        
        
        private void button2_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text;
            string hotenSV = txtHotenSV.Text;
            DateTime ngaySinh = dtpNgaySinh.Value;
            string maLop = cbLop.SelectedValue.ToString();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hotenSV))
            {
                MessageBox.Show("Mã sinh viên và họ tên không được để trống.");
                return;
            }

            using (var context = new Model1())
            {
                var newSV = new Sinhvien
                {
                    MaSV = maSV,
                    HotenSV = hotenSV,
                    NgaySinh = ngaySinh,
                    MaLop = maLop
                };

                context.Sinhviens.Add(newSV);
                context.SaveChanges();  // Lưu dữ liệu vào cơ sở dữ liệu
            }

            LoadData();  // Làm mới dữ liệu trong DataGridView
            MessageBox.Show("Thêm sinh viên thành công!");
            ClearFields();  // Xóa các trường dữ liệu sau khi thêm
                            // Bật các nút Lưu và Không Lưu
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                string maSV = dgvSinhvien.SelectedRows[0].Cells["MaSV"].Value.ToString();

                using (var context = new Model1())
                {
                    var sv = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);
                    if (sv != null)
                    {
                        sv.HotenSV = txtHotenSV.Text;
                        sv.NgaySinh = dtpNgaySinh.Value;
                        sv.MaLop = cbLop.SelectedValue.ToString();

                        context.SaveChanges();  // Cập nhật dữ liệu vào cơ sở dữ liệu
                        MessageBox.Show("Sửa thông tin sinh viên thành công!");
                    }
                }

                LoadData();  // Làm mới dữ liệu trong DataGridView
                             // Bật các nút Lưu và Không Lưu
                btnLuu.Enabled = true;
                btnKhongLuu.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                string maSV = dgvSinhvien.SelectedRows[0].Cells["MaSV"].Value.ToString();

                using (var context = new Model1())
                {
                    var sv = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);
                    if (sv != null)
                    {
                        context.Sinhviens.Remove(sv);
                        context.SaveChanges();  // Xóa sinh viên khỏi cơ sở dữ liệu
                        MessageBox.Show("Xóa sinh viên thành công!");
                        // Bật các nút Lưu và Không Lưu
                        btnLuu.Enabled = true;
                        btnKhongLuu.Enabled = true;
                        LoadData();  // Làm mới dữ liệu trong DataGridView
                    }
                }
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Nếu người dùng không muốn lưu, chỉ cần load lại dữ liệu từ cơ sở dữ liệu
            LoadData();

            // Tắt nút Lưu và Không Lưu
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;

            // Xóa các trường dữ liệu để người dùng có thể chỉnh sửa tiếp
            ClearFields();
        }

        private void dgvSinhvien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                txtMaSV.Text = dgvSinhvien.SelectedRows[0].Cells["MaSV"].Value.ToString();
                txtHotenSV.Text = dgvSinhvien.SelectedRows[0].Cells["HotenSV"].Value.ToString();
                dtpNgaySinh.Value = DateTime.Parse(dgvSinhvien.SelectedRows[0].Cells["NgaySinh"].Value.ToString());
                cbLop.Text = dgvSinhvien.SelectedRows[0].Cells["Lop"].Value.ToString();
            }
        }

        private void dgvSinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenSinhVien = txtTenSinhVien.Text.Trim(); // Lấy giá trị từ TextBox tìm kiếm
            if (!string.IsNullOrEmpty(tenSinhVien))
            {
                // Thực hiện tìm kiếm trong cơ sở dữ liệu
                using (var context = new Model1())
                {
                    var sinhvienList = context.Sinhviens
                                              .Where(sv => sv.HotenSV.Contains(tenSinhVien))
                                              .Include(sv => sv.Lop)
                                              .ToList();

                    // Cập nhật DataGridView với kết quả tìm kiếm
                    dgvSinhvien.DataSource = sinhvienList.Select(sv => new
                    {
                        sv.MaSV,
                        sv.HotenSV,
                        sv.NgaySinh,
                        Lop = sv.Lop.TenLop
                    }).ToList();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên để tìm kiếm.");
            }
        }

        private void frmSinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn "No", thì không đóng form
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy bỏ việc đóng form
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra người dùng chọn Yes hay No
            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Đóng ứng dụng
            }
        }
    }
}
