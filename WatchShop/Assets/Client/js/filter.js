var checked = "";
var priceFilter = "";
var discountFilter = "";

$(document).ready(function () {

    $('.branch-checkbox').on('change', function () {
        if ($(this).val() != "all") {
            if ($(this).prop('checked') == true) {
                checked += $(this).val() + " ";
            } else {
                var tmp = $(this).val();
                checked = checked.replace(tmp, '');
            }

            $.ajax({
                type: "GET",
                url: "/Client/Products/Filter",
                data: {
                    branchFilter: checked,
                    priceFilter: priceFilter,
                    discountFilter: discountFilter
                },
                success: function (response) {
                    window.location.href = "/Client/Products/Index";
                }
            });
        } else {
            checked = "";
            if ($(this).prop('checked') == false) {
                $(this).prop('checked', true);
            } else {
                $('.branch-checkbox').not(this).prop('checked', false);
            }

            $.ajax({
                type: "GET",
                url: "/Client/Products/Filter",
                data: {
                    branchFilter: checked
                },
                success: function (response) {
                    window.location.href = "/Client/Products/Index";
                }
            });
        }
    });

    $('.price-radio').on('change', function () {
        priceFilter = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Client/Products/Filter",
            data: {
                branchFilter: checked,
                priceFilter: priceFilter,
                discountFilter: discountFilter
            },
            success: function (response) {
                window.location.href = "/Client/Products/Index";
            }
        });
    });

    $('.discount-radio').click(function () {
        discountFilter = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Client/Products/Filter",
            data: {
                branchFilter: checked,
                priceFilter: priceFilter,
                discountFilter: discountFilter
            },
            success: function (response) {
                window.location.href = "/Client/Products/Index";
            }
        });
    });

    updatePriceRadio();
    updateCheckbox();
    updateDiscountRadio();
});

function updateCheckbox() {
    $.ajax({
        type: "GET",
        url: "/Client/Products/GetAllBranchFilter",
        success: function (response) {
            checked = response;
            if (response == null || response == "") {
                $('#checkbox-all').attr("checked", true);
            } else {
                var filters = response.trim().split(' ');
                for (var i = 0; i < filters.length; i++) {
                    var tmp = "#" + filters[i];
                    $(tmp).prop('checked', true);
                }
            }
        }
    });
}

function updatePriceRadio() {
    $.ajax({
        type: "GET",
        url: "/Client/Products/GetPriceFilter",
        success: function (response) {
            priceFilter = response;
            var id = "#" + priceFilter;
            $(id).prop('checked', true);
        }
    });
}

function updateDiscountRadio() {
    $.ajax({
        type: "GET",
        url: "/Client/Products/GetDiscountFilter",
        success: function (response) {
            discountFilter = response;
            var id = "#" + discountFilter;
            $(id).prop('checked', true);
        }
    });
}