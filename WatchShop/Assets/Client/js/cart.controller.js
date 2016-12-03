$(document).ready(function () {
    var oldQuantity;

    $('#button-continue-shopping').off('click').on('click', function () {
        window.location.href = "Client/Products/Index"
    });

    // Update total cost when user change quantity
    $('.quantity').on('mousedown', function () {
        oldQuantity = $(this).val();
    });

    $('.quantity').on('input', function () {
        var quantity = $(this).val();
        if (quantity != null && quantity != "") {
            var totalCostId = "#" + $(this).data('id') + "-total-cost";
            var totalCost = $(this).data('bind') * quantity;
            $(totalCostId).text(totalCost);
        }
    });

    $('.quantity').on('focusout', function () {
        var quantity = $(this).val();
        if (quantity == null || quantity == 0) {
            $(this).val(oldQuantity);
        }
    });

    // Update cart
    $('#button-update-cart').off('click').on('click', function () {
        var products = $('.quantity');
        var cart = [];
        $.each(products, function (i, item) {
            cart.push({
                Quantity: $(item).val(),
                Product: {
                    Id: $(item).data('id')
                }
            });
        });

        $.ajax({
            url: '/Client/Cart/Update',
            data: {
                cartModel: JSON.stringify(cart)
            },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
                if (response.status = true) {
                    window.location.href = "/Client/Cart/Index";
                }
            }
        });
    });

    // Delete cart
    $('#button-delete-cart').off('click').on('click', function () {
        $.ajax({
            url: '/Client/Cart/DeleteAll',
            type: 'GET',
            success: function (response) {
                if (response.status = true) {
                    window.location.href = "/Client/Cart/Index";
                }
            }
        });
    });
});