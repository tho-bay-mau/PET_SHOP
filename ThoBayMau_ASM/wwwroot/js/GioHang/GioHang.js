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

        var tongTien = document.getElementById(`t ${id}`)
        $.ajax({
            url: '/GioHang/TangSoLuong',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (result) {
                let vndString = result.tongTienSanPham + "đ";

                let tamTinh = result.tamTinh + "đ";

                let Tong = result.tongTien + "đ";

                console.log(result);

                tongTien.innerHTML = vndString;
                tamtinh.innerHTML = tamTinh;
                tongtien.innerHTML = Tong;
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
                    let vndString = result.tongTienSanPham+"đ";

                    let tamTinh = result.tamTinh + "đ";

                    let Tong = result.tongTien + "đ";

                    console.log(result);

                    tongTien.innerHTML = vndString;
                    tamtinh.innerHTML = tamTinh;
                    tongtien.innerHTML = Tong;
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
                console.log(result);
                let vndString = result.tongTienSanPham.toLocaleString().replace(/,/g, ',') + ',000 ₫';

                let tamTinh = result.tamTinh.toLocaleString().replace(/,/g, ',') + ',000 ₫';

                let Tong = result.tongTien.toLocaleString().replace(/,/g, ',') + ',000 ₫';

                tongTien.innerHTML = vndString;

                tamtinh.innerHTML = tamTinh;

                tongtien.innerHTML = Tong;
            }
        })
       
    })
}

       