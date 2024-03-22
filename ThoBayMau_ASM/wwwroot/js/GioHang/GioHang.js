var tang = document.querySelectorAll('.tang')

var giam = document.querySelectorAll('.giam')


var tamtinh = document.getElementById('TamTinh')

var tongtien = document.getElementById('TongTien')

for (var i = 0; i < tang.length; i++) {
    tang[i].addEventListener('click', function () {
        var id = this.dataset.id;
        var soluong = document.getElementById(`sl_${id}`)

        var sl = soluong.value
        var slTang = ++sl
        soluong.value = slTang;
        var tongTien = document.getElementById(`t ${id}`)

        $.ajax({
            url: '/GioHang/TangSoLuong',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (result) {
                let vndString = result.tongTienSanPham;
                let formattedVndString = new Intl.NumberFormat('vi-VN').format(vndString);
                let tamTinh = result.tamTinh;
                let formattedTamTinh = new Intl.NumberFormat('vi-VN').format(tamTinh);
                let Tong = result.tongTien;
                let formattedTong = new Intl.NumberFormat('vi-VN').format(Tong);
                console.log(formattedVndString);

                tongTien.innerHTML = formattedVndString + 'đ';
                tamtinh.innerHTML = formattedTamTinh + 'đ';
                tongtien.innerHTML = formattedTong + 'đ';
            }
        })
    }); 
}

for (var i = 0; i < giam.length; i++) {
    giam[i].addEventListener('click', function () {
        var id = this.dataset.id;
        var soluong = document.getElementById(`sl_${id}`)
        var sl = soluong.value
        if (sl == 1) {

        }
        else {
            var slTang = --sl
            soluong.value = slTang;
            var tongTien = document.getElementById(`t ${id}`)
            $.ajax({
                url: '/GioHang/GiamSoLuong',
                type: 'GET',
                data: { id: id },
                dataType: 'json',
                success: function (result) {
                    let vndString = result.tongTienSanPham;
                    let formattedVndString = new Intl.NumberFormat('vi-VN').format(vndString);
                    let tamTinh = result.tamTinh;
                    let formattedTamTinh = new Intl.NumberFormat('vi-VN').format(tamTinh);
                    let Tong = result.tongTien;
                    let formattedTong = new Intl.NumberFormat('vi-VN').format(Tong);
                    console.log(formattedVndString);

                    tongTien.innerHTML = formattedVndString + 'đ';
                    tamtinh.innerHTML = formattedTamTinh + 'đ';
                    tongtien.innerHTML = formattedTong + 'đ';
                }
            })
        }
    });
}

var txt__soluong = document.querySelectorAll('.txt__soluong'); 

for (var i = 0; i < txt__soluong.length; i++) {
    txt__soluong[i].addEventListener("input", function (event) {
        var id = this.dataset.id;

        var inputValue = event.target.value;
        var numericValue = inputValue.replace(/\D/g, ''); 
        event.target.value = numericValue;

        var tongTien = document.getElementById(`t ${id}`)

        $.ajax({
            url: '/GioHang/InputSoLuong',
            type: 'GET',
            data: { id: id, SoLuong: numericValue },
            dataType: 'json',
            success: function (result) {
                let vndString = result.tongTienSanPham;
                let formattedVndString = new Intl.NumberFormat('vi-VN').format(vndString);
                let tamTinh = result.tamTinh;
                let formattedTamTinh = new Intl.NumberFormat('vi-VN').format(tamTinh);
                let Tong = result.tongTien;
                let formattedTong = new Intl.NumberFormat('vi-VN').format(Tong);
                console.log(formattedVndString);

                tongTien.innerHTML = formattedVndString + 'đ';
                tamtinh.innerHTML = formattedTamTinh + 'đ';
                tongtien.innerHTML = formattedTong + 'đ';
            }
        })
       
    })
}

       