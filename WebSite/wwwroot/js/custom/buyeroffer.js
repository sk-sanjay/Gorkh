try {
    $(function () {
        $('a.details').on('click', function (event) {
            //debugger
            var url = "/ProductDetails?handler=product";
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

        $('#btnsubmit').click(function (event) {
            SubmitForm(event);
        });
    });
    function isNumberKey(a) {
        var x = a.val();
        a.val(x.replace(/[^0-9\.]/g, ''));
    }

    function SubmitForm(event) {
        debugger
        var ctrlOldHtml = $('#exampleModal #btnsubmit').html();
        var theForm = $('#exampleModal #btnsubmit').parents('form:first');
        if (theForm.valid()) {
            var price = $('#exampleModal #txt_price').val();

            if (price == "") {
                //$("#divClientAlert").addClass("alert-danger");
                //$("#divClientAlert > p.m-0").text("Price is required");
                //$("#divClientAlert").show();
                $('#exampleModal #txt_price').focus();
                alert("Please Enter Your Price!");
                //SetTimeOut($("#divClientAlert"));
                //e.preventDefault();
                return false;
            }
            else {
                //$('#exampleModal #btnsubmit').attr('disabled', true);
                //$('#exampleModal #btnsubmit').html('<i class="fa fa-spinner fa-spin"></i> Please wait');
                theForm.submit();
                return true;
            }
        } else {
            event.preventDefault();
            $('#exampleModal #btnsubmit').attr('disabled', false);
            $('#exampleModal #btnsubmit').html(ctrlOldHtml);
            return false;
        }
    }
}
catch (e) {
    console.log(e.message);
}