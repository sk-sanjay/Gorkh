try {
    
    /*Header Search*/
    $('#btnSearch1').on('click', function (e) {

        var keyword = $('#txtkeyword').val();
        var catId = $('#ddlCategoryHeader').val();
        var url = "ProductsBySubCat?category_id=" + encodeURIComponent(catId) + "&keyword=" + encodeURIComponent(keyword);
        window.location.href = url;
    });
}
catch (e) {
    console.log(e.message);
}