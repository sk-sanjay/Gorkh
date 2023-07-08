try {
    $("#tblDataTable").DataTable({
        initComplete: function () {
            var r = $("#tblDataTable tfoot tr");
            r.find("th").each(function () {
                if ($(this).hasClass("search")) {
                    $(this).css("padding", 5);
                    var title = $(this).text();
                    $(this).html('<input type="text" class="form-control" placeholder="' + title + '" />');
                } else {
                    $(this).html("");
                }
            });
            $("#tblDataTable thead").prepend(r);
            this.api().columns().every(function () {
                var that = this;
                $("input", this.footer()).on("keyup change clear",
                    function () {
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });
            });
        }
    });

    $("#CountryId").on("change", function () {
        debugger;
        var countryid = $("#CountryId option:selected").val();
        if (countryid != null) {
            $.getJSON('?handler=GetState&cid=' + countryid + '', (data) => {
                $("#StateId").empty();
                if (data != '' && data != null) {
                    /*  $("#ddlSubCategoryId").show();*/
                    var item1 = "<option value='Select'>Select</option>";
                    $.each(data, function (i, item) {
                        item1 += `<option value="${item.id}">${item.stateName}</option>`;
                    });
                    $("#StateId").append(item1);

                }
                else {
                    /*  $("#ddlSubCategoryId").hide();*/
                    var item1 = "<option value='Select'>Select</option>";
                    $("#StateId").append(item1);
                }
            });
        }
    })


    function GetCountries() {
        $.get("/Admin/Masters/Cities/Index?handler=Country")
            .done(function (data) {
                var options = '<option value="">-select-</option>';
                if (data && data != "Category not found") {
                    $(data).each(function (i, e) {
                        options += '<option value="' + e.id + '">' + e.text + "</option>";
                    });
                } else {
                    $("#divClientAlert").addClass("alert-danger");
                    $("#divClientAlert > p.m-0").text(data);
                    $("#divClientAlert").show();
                    SetTimeOut($("#divClientAlert"));

                }
                $("select#CountryId")
                    .empty()

                    .append(options);
            });
    };

    $("#ManageModal").on("show.bs.modal",
        function (event) {
            GetCountries();
            var url = "/Admin/Masters/Cities/Index?handler=Model";
            var id = $(event.relatedTarget).data("id");
            if (typeof id != "undefined") {
                $.ajax({
                    type: "GET",
                    url: url,
                    data: { id: id },
                    success: function (data) {
                        debugger;
                        if (data == 'unauthorized') {
                            // reload
                        }
                        else if (data != null) {
                            if (data.modelDto != null) {
                                $("#Id").val(data.modelDto.id);
                                $("#CountryId").val(data.modelDto.countryId);


                                $("#CityName").val(data.modelDto.cityName);
                                $("#CreatedDate").val(data.modelDto.createdDate);
                                $("#CreatedBy").val(data.modelDto.createdBy);
                                $("#IsActive").attr("checked", data.modelDto.isActive);

                                $("#StateId").empty();
                                var item1 = "<option value='Select'>Select</option>";

                                if (data.stateDropDown != null && data.stateDropDown.length > 0) {
                                    $.each(data.stateDropDown, function (i, item) {
                                        item1 += `<option value="${item.value}">${item.text}</option>`;
                                    });
                                    console.log(item1);
                                    $("#StateId").append(item1);

                                }
                                $("#StateId").val(data.modelDto.stateId);
                            }


                        }
                    }
                });
            } else {
                $("#Id").val("0");
                $("#CountryId").val('');
                $("#StateId").val('');
                $("#CityName").val('');
                $("#CreatedDate").val((new Date()).toISOString().split('T')[0]);
                $("#IsActive").attr("checked", true);
            }
        });
    $('#btnSave').click(function (event) {
        var theForm = $(this).parents('form:first');
        if (theForm.valid()) {
            getConfirm('Are you sure?', function (result) {
                result === true ? theForm.submit() : event.preventDefault();
            });
        } else {
            event.preventDefault();
        }
    });
    function Delete(id) {
        getConfirm("Are you sure you want to delete?", function (result) {
            if (result) {
                $.ajax({
                    type: "GET",
                    url: "/Admin/Masters/Cities/Index?handler=Delete",
                    data: { id: id },
                    success: function (data) {
                        if (data != null) {
                            if (data === "unauthorized") {
                                window.location.href = "/Account/Login";
                            } else if (data === "success") {
                                window.location.reload(true);
                            }
                        }
                    }
                });
            }
        });
    }
}
catch (e) {
    console.log(e.message);
}