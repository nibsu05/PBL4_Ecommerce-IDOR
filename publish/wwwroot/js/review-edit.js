function initEditReviewModal() {
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
            const reviewId = document.getElementById("reviewId").value;
            const rating = parseInt(starContainer.dataset.selected);
            console.log("Sending reviewId:", reviewId);
            console.log("Content:", content);
            console.log("Rating:", rating);
            fetch('/Buyer/EditComment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    reviewId: reviewId,
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
