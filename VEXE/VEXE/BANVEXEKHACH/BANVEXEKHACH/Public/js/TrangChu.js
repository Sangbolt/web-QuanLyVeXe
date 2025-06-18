document.addEventListener("DOMContentLoaded", function () {
    // Khuyến Mãi Nổi Bật Carousel
    function initPromotionCarousel() {
        const carousel = document.getElementById("carouselExampleIndicators");
        const indicators = carousel.querySelectorAll(".carousel-indicators li");
        const slides = carousel.querySelectorAll(".carousel-item");
        let currentIndex = 0;
        let autoSlideInterval;

        function showSlide(index) {
            if (index === currentIndex) return;

            // Cập nhật trạng thái của các slides và indicators
            slides[currentIndex].classList.remove("active");
            indicators[currentIndex].classList.remove("active");

            slides[index].classList.add("active");
            indicators[index].classList.add("active");

            currentIndex = index;
        }

        function nextSlide() {
            let nextIndex = (currentIndex + 1) % slides.length; // Lặp lại slide đầu khi kết thúc
            showSlide(nextIndex);
        }

        function startAutoSlide() {
            stopAutoSlide(); // Xóa interval trước khi bắt đầu mới
            autoSlideInterval = setInterval(nextSlide, 3000); // Chuyển slide đúng 3 giây
        }

        function stopAutoSlide() {
            clearInterval(autoSlideInterval);
        }

        // Gán sự kiện click vào indicators
        indicators.forEach((indicator, index) => {
            indicator.addEventListener("click", () => {
                stopAutoSlide();
                showSlide(index);
                startAutoSlide();
            });
        });

        // Dừng auto-slide khi hover
        carousel.addEventListener("mouseenter", stopAutoSlide);
        carousel.addEventListener("mouseleave", startAutoSlide);

        startAutoSlide(); // Bắt đầu auto-slide
    }

    // Tin Tức Mới Carousel
    function initNewsCarousel() {
        const carousel = document.getElementById("newsCarousel");
        const indicators = carousel.querySelectorAll(".carousel-indicators li");
        const slides = carousel.querySelectorAll(".carousel-item");
        let currentIndex = 0;
        let autoSlideInterval;

        function showSlide(index) {
            if (index === currentIndex) return;

            // Cập nhật trạng thái của các slides và indicators
            slides[currentIndex].classList.remove("active");
            indicators[currentIndex].classList.remove("active");

            slides[index].classList.add("active");
            indicators[index].classList.add("active");

            currentIndex = index;
        }

        function nextSlide() {
            let nextIndex = (currentIndex + 1) % slides.length; // Lặp lại slide đầu khi kết thúc
            showSlide(nextIndex);
        }

        function startAutoSlide() {
            stopAutoSlide(); // Xóa interval trước khi bắt đầu mới
            autoSlideInterval = setInterval(nextSlide, 3000); // Chuyển slide đúng 3 giây
        }

        function stopAutoSlide() {
            clearInterval(autoSlideInterval);
        }

        // Gán sự kiện click vào indicators
        indicators.forEach((indicator, index) => {
            indicator.addEventListener("click", () => {
                stopAutoSlide();
                showSlide(index);
                startAutoSlide();
            });
        });

        // Dừng auto-slide khi hover
        carousel.addEventListener("mouseenter", stopAutoSlide);
        carousel.addEventListener("mouseleave", startAutoSlide);

        startAutoSlide(); // Bắt đầu auto-slide
    }

    // Khởi tạo cả 2 carousel
    initPromotionCarousel();
    initNewsCarousel();
});