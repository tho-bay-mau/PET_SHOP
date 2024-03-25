//$('#keyword').keyup(function () {
//    var searchField = $('#keyword').val();
//    var expression = RegExp(searchField, "i");
//    $('.tt-dataset').remove();
//    $.ajax({
//        type: "GET",
//        url: "/Home/Search",
//        success: function (response) {
//            var data = JSON.parse(response);
//            console.log(data);
//            if (searchField != "") {
//                var html_Body = `<div class="tt-dataset tt-dataset-states">

//                        </div>`;
//            } $('.tt-menu').append(html_Body);
//            $.each(data, function (key, item) {
//                if (item.Ten.search(expression) != -1 && searchField != "") {
//                    var imageUrl = "";
//                    if (item.Anhs.length > 0) {
//                        imageUrl = "../../img/products/" + item.Anhs[0].TenAnh;
//                    }
//                    var html_Search = `<div class="man-section tt-suggestion tt-selectable">
//                                <div class="image-section">
//                                    <img src="${imageUrl}">
//                                </div>
//                                <div class="description-section">
//                                    <h1>${item.Ten}</h1>
//                                    <p>Avengers: Endgame is an American superhero film released in 2019, produced by Marvel Studios and distributed by Walt Disney Studios Motion Pictures.</p>
//                                    <span>
//                                        <i class="fa fa-clock-o" aria-hidden="true">
//                                        </i> 12:00 PM <i class="fa fa-map-marker" aria-hidden="true"></i> Ha Noi
//                                    </span><div class="more-section">
//                                        <a href="#" target="_blank">
//                                            <button>More Info</button>
//                                        </a>
//                                    </div>
//                                </div>
//                                <div style="clear:both;">
//                                </div>
//                            </div>`;
//                    $('.tt-dataset').append(html_Search);
//                }
//            })
//        }
//    })
//})

var debounceTimer;
$('#keyword').on('input', function () {
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(function () {
        console.log($('#keyword').val());
        var searchField = $('#keyword').val().toLowerCase();
        var accent_map = {
            'á': 'a', 'à': 'a', 'ả': 'a', 'ã': 'a', 'ạ': 'a',
            'ă': 'a', 'ắ': 'a', 'ằ': 'a', 'ẳ': 'a', 'ẵ': 'a', 'ặ': 'a',
            'â': 'a', 'ấ': 'a', 'ầ': 'a', 'ẩ': 'a', 'ẫ': 'a', 'ậ': 'a',
            'đ': 'd',
            'é': 'e', 'è': 'e', 'ẻ': 'e', 'ẽ': 'e', 'ẹ': 'e',
            'ê': 'e', 'ế': 'e', 'ề': 'e', 'ể': 'e', 'ễ': 'e', 'ệ': 'e',
            'í': 'i', 'ì': 'i', 'ỉ': 'i', 'ĩ': 'i', 'ị': 'i',
            'ó': 'o', 'ò': 'o', 'ỏ': 'o', 'õ': 'o', 'ọ': 'o',
            'ô': 'o', 'ố': 'o', 'ồ': 'o', 'ổ': 'o', 'ỗ': 'o', 'ộ': 'o',
            'ơ': 'o', 'ớ': 'o', 'ờ': 'o', 'ở': 'o', 'ỡ': 'o', 'ợ': 'o',
            'ú': 'u', 'ù': 'u', 'ủ': 'u', 'ũ': 'u', 'ụ': 'u',
            'ư': 'u', 'ứ': 'u', 'ừ': 'u', 'ử': 'u', 'ữ': 'u', 'ự': 'u',
            'ý': 'y', 'ỳ': 'y', 'ỷ': 'y', 'ỹ': 'y', 'ỵ': 'y'
        };
        searchField = searchField.replace(/[^\u0000-\u007E]/g, function (c) {
            return accent_map[c] || c;
        });
        var expression = RegExp(searchField, "i");
        $('.tt-dataset').remove();
        $.ajax({
            type: "GET",
            url: "/Home/Search",
            success: function (response) {
                var data = JSON.parse(response);
                console.log(data);
                if (searchField != "") {
                    var html_Body = `<div class="tt-dataset tt-dataset-states"></div>`;
                    $('.tt-menu').append(html_Body);
                }
                var count = 0;
                $.each(data, function (key, item) {
                    var itemName = item.Ten.toLowerCase();
                    itemName = itemName.replace(/[^\u0000-\u007E]/g, function (c) {
                        return accent_map[c] || c;
                    });
                    if (itemName.search(expression) != -1 && searchField != "" && count < 3) {
                        var imageUrl = "";
                        if (item.Anhs.length > 0) {
                            imageUrl = "../../img/products/" + item.Anhs[0].TenAnh;
                        }
                        var html_Search = `<div class="man-section tt-suggestion tt-selectable">
                                <a class"product-search" href="/Home/product_detail?id_sp=${item.Id}">
                                    <div class="image-section">
                                        <img src="${imageUrl}">
                                    </div>
                                    <div class="description-section">
                                        <h1>${item.Ten}</h1>
                                        <p>${item.Mota}</p>
                                    </div>
                                    <div style="clear:both;"></div>
                                </a>
                            </div>`;
                        $('.tt-dataset').append(html_Search);
                        count++;
                    }
                })
            }
        })
    }, 300);
});