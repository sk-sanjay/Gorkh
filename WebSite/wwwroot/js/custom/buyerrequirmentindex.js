try {
        $(function () {
            $('a.BuyerReqdetails').on('click', function (event) {
                debugger
                var url = "/index?handler=BuyerRequiremenData";
                var id = $(this).data('id');
                if (typeof id != "undefined") {
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { id: id },
                        success: function (data) {
                            debugger;
                            if (data != null) {
                                $('#LblTitle').text(data.productListingTitle);
                                $('#Category').text(data.category);
                                $('#SubCategory').text(data.subCategory);
                                $('#SubSubCategory').text(data.subSubCategoryName);
                                $('#AddedOn').text(data.createdDateStr);
                                $('#Description').text(data.descrition);
                                $("#exampleModal12 .modal-title").html(data)
                                $("#exampleModal12").modal();
                                
                            }
                        },
                        complete: function () {
                            $('.modal').on('shown.bs.modal', function (e) {
                                $('.slider-for, .slider-nav').slick('setPosition');
                                $('.wrap-modal-slider').addClass('open');
                            })
                            $('.slider-for').slick({
                                slidesToShow: 1,
                                slidesToScroll: 1,
                                arrows: true,
                                //fade: true,
                                asNavFor: '.slider-nav'
                            });
                            $('.slider-nav').slick({
                                slidesToShow: 4,
                                slidesToScroll: 1,
                                asNavFor: '.slider-for',
                                //dots: true,
                                arrows: false,
                                //centerMode: true,
                                focusOnSelect: true
                            });
                        }
                    });
                } else {

                }
            });

        
        });
    //$(function () {
    //    $('a.details').on('click', function (event) {
    //        debugger;
    //        //$('.modal-body').load("/ProductsBySubCat/product?id=${$(this).data('id')}");
    //        var url = "/Index?handler=product";
    //        var id = $(this).data('id');
    //        if (typeof id != "undefined") {
    //            $.ajax({
    //                type: "GET",
    //                url: url,
    //                data: { id: id },
    //                success: function (data) {
    //                    if (data != null) {
    //                        $('.modal-body').html(data);
    //                        $("#exampleModal").modal("show");
    //                    }
    //                },
    //                complete: function () {
    //                    $('.modal').on('shown.bs.modal', function (e) {
    //                        $('.slider-for, .slider-nav').slick('setPosition');
    //                        $('.wrap-modal-slider').addClass('open');
    //                    })
    //                    $('.slider-for').slick({
    //                        slidesToShow: 1,
    //                        slidesToScroll: 1,
    //                        arrows: true,
    //                        //fade: true,
    //                        asNavFor: '.slider-nav'
    //                    });
    //                    $('.slider-nav').slick({
    //                        slidesToShow: 4,
    //                        slidesToScroll: 1,
    //                        asNavFor: '.slider-for',
    //                        //dots: true,
    //                        arrows: false,
    //                        //centerMode: true,
    //                        focusOnSelect: true
    //                    });
    //                }
    //            });
    //        } else {

    //        }
    //    });
    //});
   

}
catch (e) {
    console.log(e.message);
}