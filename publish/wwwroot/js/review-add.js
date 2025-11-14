function initCreateReviewModal() {
    const stars = document.querySelectorAll(".star-rating .star");
    const starContainer = document.getElementById("star-rating");
    if (!starContainer) return;

    let selectedRating = parseInt(starContainer.dataset.selected);

    function highlightStars(rating) {
        stars.forEach(star => {
            star.classList.remove("filled", "hovered");
            if (parseInt(star.dataset.value) <= rating) {
                star.classList.add("hovered");
            }
        });
    }

    function setStars(rating) {
        stars.forEach(star => {
            star.classList.remove("filled");
            if (parseInt(star.dataset.value) <= rating) {
                star.classList.add("filled");
            }
        });
        starContainer.dataset.selected = rating;
        selectedRating = rating;
    }

    stars.forEach(star => {
        star.addEventListener("mouseover", function () {
            highlightStars(parseInt(this.dataset.value));
        });

        star.addEventListener("mouseout", function () {
            setStars(selectedRating);
        });

        star.addEventListener("click", function () {
            setStars(parseInt(this.dataset.value));
        });
    });

    const submitButton = document.getElementById("submit-review");
    if (submitButton) {
        submitButton.addEventListener("click", function () {
            const content = document.getElementById("content").value;
            const productId = document.getElementById("productId").value;
            const rating = parseInt(starContainer.dataset.selected);
            console.log("Sending productId:", productId);
            console.log("Content:", content);
            console.log("Rating:", rating);
            fetch('/Buyer/WriteComment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    productId: productId,
                    content: content,
                    rating: rating
                })
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    alert("Đánh giá đã được cập nhật!");
                    location.reload();
                } else {
                    alert("Có lỗi xảy ra: " + data.message);
                }
            })
            .catch(err => {
                console.error("Error:", err);
                alert("Không thể gửi đánh giá.");
            });
        });
    }
}
