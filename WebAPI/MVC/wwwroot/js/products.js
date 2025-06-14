document.querySelectorAll(".products").forEach(el => {
    el.addEventListener("click", (e) => {
        e.preventDefault();
        const productCard = el.closest('.product-card');
        if (!productCard.classList.contains('product-selected')) {
            const basket_span = document.getElementById('basket-span');
            basket_span.style.display = 'flex';
            basket_span.innerHTML = parseInt(basket_span.innerHTML || 0) + 1;
            const productId = productCard.getAttribute('data-id');
            productCard.classList.add('product-selected');
            const basket = sessionStorage.getItem("basket");
            if (basket) {
                const object = JSON.parse(basket);
                object.push({id: productId, quantity: 1});
                sessionStorage.setItem("basket", JSON.stringify(object));
            } else {
                const object = [{id: productId, quantity: 1}];
                sessionStorage.setItem("basket", JSON.stringify(object));
            }
        }
    });
});

document.addEventListener('click', function (e) {
    const basket_span = document.getElementById('basket-span');

    if (e.target.classList.contains('minus-btn')) {
        const input = e.target.nextElementSibling;
        if (parseInt(input.value) > 1) {
            input.value = parseInt(input.value) - 1;
            updateBasketQuantity(e.target.closest('.product-card'));
        } else {
            const card = e.target.closest('.product-card');
            const productId = card.getAttribute('data-id');
            const data = sessionStorage.getItem("basket");
            const basket = JSON.parse(data);
            const filtered_basket = basket.filter(p => p.id != productId);
            if (filtered_basket.length == 0) {
                basket_span.style.display = 'none';
            }
            sessionStorage.setItem("basket", JSON.stringify(filtered_basket));
            card.classList.remove('product-selected');
        }
        basket_span.innerHTML = parseInt(basket_span.innerHTML) - 1;

    }

    if (e.target.classList.contains('plus-btn')) {
        const input = e.target.previousElementSibling;
        if (parseInt(input.value) < input.max) {
            input.value = parseInt(input.value) + 1;
            basket_span.innerHTML = parseInt(basket_span.innerHTML) + 1;
            updateBasketQuantity(e.target.closest('.product-card'));
        }
    }

});

function updateBasketQuantity(productCard) {
    const productId = productCard.getAttribute('data-id');
    const quantity = parseInt(productCard.querySelector('.quantity-input').value);

    const basket = sessionStorage.getItem("basket");
    let object = basket ? JSON.parse(basket) : [];

    const productIndex = object.findIndex(p => p.id == productId);

    if (productIndex === -1) {
        object.push({id: productId, quantity: quantity});
    } else {
        object[productIndex].quantity = quantity;
    }

    sessionStorage.setItem("basket", JSON.stringify(object));
}

// Initialize product cards based on basket
document.addEventListener('DOMContentLoaded', function () {
    const basket = sessionStorage.getItem("basket");
    if (basket) {
        const object = JSON.parse(basket);
        object.forEach(item => {
            console.log("in")
            const productCard = document.querySelector(`.product-card[data-id="${item.id}"]`);
            if (productCard) {
                productCard.classList.add('product-selected');
                const quantityInput = productCard.querySelector('.quantity-input');
                quantityInput.value = item.quantity;
            }
        });
    }
});
