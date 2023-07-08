try {
    $(document).ready(function () {
        //Pagination
        pageSize = 21;
        incremSlide = 5;
        startPage = 0;
        numberPage = 0;

        var pageCount = $(".line-content").length / pageSize;
        var totalSlidepPage = Math.floor(pageCount / incremSlide);

        for (var i = 0; i < pageCount; i++) {
            $("#pagin").append('<li><a href="#">' + (i + 1) + '</a></li> ');
            if (i > pageSize) {
                $("#pagin li").eq(i).hide();
            }
        }

        var prev = $("<li/>").addClass("prev").html("Prev").click(function () {
            startPage -= 5;
            incremSlide -= 5;
            numberPage--;
            slide();
        });

        prev.hide();

        var next = $("<li/>").addClass("next").html("Next").click(function () {
            startPage += 5;
            incremSlide += 5;
            numberPage++;
            slide();
        });

        $("#pagin").prepend(prev).append(next);

        $("#pagin li").first().find("a").addClass("current");

        slide = function (sens) {
            $("#pagin li").hide();

            for (t = startPage; t < incremSlide; t++) {
                $("#pagin li").eq(t + 1).show();
            }
            if (startPage == 0) {
                next.show();
                prev.hide();
            } else if (numberPage == totalSlidepPage) {
                next.hide();
                prev.show();
            } else {
                next.show();
                prev.show();
            }


        }

        showPage = function (page) {
            $(".line-content").hide();
            $(".line-content").each(function (n) {
                if (n >= pageSize * (page - 1) && n < pageSize * page)
                    $(this).show();
            });
        }

        showPage(1);
        $("#pagin li a").eq(0).addClass("current");

        $("#pagin li a").click(function () {
            $("#pagin li a").removeClass("current");
            $(this).addClass("current");
            showPage(parseInt($(this).text()));
        });
    });



    $(document).ready(function () {
        $('.selectInd1').select2({
            placeholder: "Select Sub Category",
            allowClear: true
        });
    });
    $(document).ready(function () {
        $('.selectCountry').select2({
            placeholder: "Select Country",
            allowClear: true
        });
    });
    $(document).ready(function () {
        $('.selectState').select2({
            placeholder: "Select State",
            allowClear: true
        });
    });

    $("#ddlCategory").on("change", function () {
        var maincat = $("#ddlCategory option:selected").val();
        if (maincat != null) {
            $.getJSON('?handler=GetSubCategory&maincat=' + maincat + '', (data) => {
                $("#ddlSubCategory").empty();
                if (data != '' && data != null) {
                    /*  $("#ddlSubCategoryId").show();*/
                    var item1 = "<option value=''>--Select--</option>";
                    $.each(data, function (i, item) {
                        item1 += `<option value="${item.id}">${item.subCategoryName}</option>`;
                    });
                    $("#ddlSubCategory").append(item1);
                }
                else {
                    /*  $("#ddlSubCategoryId").hide();*/
                    var item1 = "<option value=''>--Select--</option>";
                    $("#ddlSubCategory").append(item1);
                }
            });
        }
    })

    $("#ddlCountry").on("change", function () {
        var maincat = $("#ddlCountry option:selected").val();
        if (maincat != null) {
            $.getJSON('?handler=GetStatebyCountryid&countryid=' + maincat + '', (data) => {
                $("#ddlState").empty();
                if (data != '' && data != null) {
                    /*  $("#ddlSubCategoryId").show();*/
                    var item1 = "<option value=''>--Select--</option>";
                    $.each(data, function (i, item) {
                        item1 += `<option value="${item.id}">${item.text}</option>`;
                    });
                    $("#ddlState").append(item1);

                }
                else {
                    /*  $("#ddlSubCategoryId").hide();*/
                    var item1 = "<option value=''>--Select--</option>";
                    $("#ddlState").append(item1);
                }
            });
        }
    })

    /*Filter data*/
    $(function () {
        var filters = {
            catId: $('#hdncategoryid').val(),
            subcatId: $('#hdnsubcatid').val(),
            countryId: null,
            stateId: null,
            saleType: 'All',
            conditionId: null,
            keyword: $('#hdnkeyword').val()
        };
        GetBreadCrumbData(filters);
        GetData(filters);
    });
    $('#btnSearch').on('click', function (e) {
        debugger
        var filters = {
            catId: $('#ddlCategory').val(),
            subcatId: $('#ddlSubCategory').val(),
            countryId: $('#ddlCountry').val(),
            stateId: $('#ddlState').val(),
            saleType: $("input[type='radio'][name='rdslaetype']:checked").val(),
            conditionId: $("input[type='radio'][name='rdcondition']:checked").val(),
            keyword: $('#hdnkeyword').val()
        };

        GetData(filters);
    });
    function GetData(filters) {
        GetBreadCrumbData(filters);
        $.ajax({
            url: '/ProductsBySubCat?handler=Search',
            type: 'Get',
            cache: false,
            async: false,
            dataType: "html",
            data: filters
        })
            .done(function (result) {
                $('#data').html(result);
            }).fail(function (xhr) {
                console.log('error : ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            });

    }
    function GetBreadCrumbData(filters) {
        $.ajax({
            url: '/ProductsBySubCat?handler=ProductsBreadCrumbs',
            type: 'Get',
            cache: false,
            async: false,
            dataType: "html",
            data: filters
        })
            .done(function (result) {
                $('#breadcrumb').html(result);
            }).fail(function (xhr) {
                console.log('error : ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            });

    }
    $(function () {
        $('a.details').on('click', function (event) {
            //debugger
            //$('.modal-body').load("/ProductsBySubCat/product?id=${$(this).data('id')}");
            var url = "/ProductsBySubCat?handler=product";
            var id = $(this).data('id');
            if (typeof id != "undefined") {
                $.ajax({
                    type: "GET",
                    url: url,
                    data: { id: id },
                    success: function (data) {
                        if (data != null) {
                            $('.modal-body').html(data);
                            $("#exampleModal").modal("show");
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
}
catch (e) {
    console.log(e.message);
}