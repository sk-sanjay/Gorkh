try {
    function Delete(id) {
        getConfirm("Are you sure you want to delete?", function (result) {
            if (result) {
                $.ajax({
                    type: "GET",
                    url: "/Admin/Masters/OurCustomers/Index?handler=Delete",
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