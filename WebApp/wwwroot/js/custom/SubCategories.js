﻿try {
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

    function GetCategories() {
        jQuery.ajaxSetup({ async: false });
        $.get("/Admin/Masters/SubCategories/Index?handler=Category")
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
                $("select#CategoryId")
                    .empty()
                    
                    .append(options);
            });
        jQuery.ajaxSetup({ async: true });
    };
    $("#ManageModal").on("show.bs.modal",
        function (event) {
            GetCategories();
            var url = "/Admin/Masters/SubCategories/Index?handler=Model";
            var id = $(event.relatedTarget).data("id");
            if (typeof id != "undefined") {
                $.ajax({
                    type: "GET",
                    url: url,
                    data: { id: id },
                    //async: false,
                    success: function (data) {
                        if (data != null) {
                            $("#Id").val(data.id);
                            $("#CategoryId").val(data.categoryId);
                            $("#SubCategoryName").val(data.subCategoryName);
                            $("#CreatedDate").val(data.createdDate);
                            $("#CreatedBy").val(data.createdBy);
                            $("#IsActive").attr("checked", data.isActive);
                        }
                    }
                });
            } else {
                $("#Id").val("0");
                $("#CategoryId").val('');
                $("#SubCategoryName").val('');
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
                    url: "/Admin/Masters/SubCategories/Index?handler=Delete",
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