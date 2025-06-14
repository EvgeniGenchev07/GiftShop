document.querySelectorAll('button[name="reorder-btn"]').forEach(btn => btn.addEventListener('click', function () {
    const card = this.closest('.order-card');
    const items = card.querySelectorAll('.order-item');
    const basket = [];
    items.forEach(item => {
        const productId = item.getAttribute('data-id');
        const quantity = parseInt(item.getAttribute('data-quantity'));
        basket.push({id: productId, quantity});
    })
    sessionStorage.setItem('basket', JSON.stringify(basket));
    window.location.href = '/Orders/Create';
}));
document.querySelectorAll('button[name="cancel-btn"]').forEach(btn => btn.addEventListener('click', function () {
    const card = this.closest('.order-card');
    const orderId = card.getAttribute('data-id');
    const data = {
        id: orderId,
        status: 'Canceled'
    }
    fetch('/Orders/ProcessOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(res => {
        if (res.ok) {
            window.location.reload();
        } else {
            alert('Something went wrong!');
        }
    }).catch(err => {
    });
}));
