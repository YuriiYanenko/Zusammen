// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
const slides = document.querySelectorAll("[data-slide]");
const buttons = document.querySelectorAll("[data-button]");

let currSlide = 0;
const maxSlide = slides.length - 1;

const updateCarousel = (number = 0) => {
    slides.forEach((slide, index) => {
        const newIndex = (index - number + slides.length) % slides.length;
        const translateValue = newIndex * (100 + 10); 
        slide.style.transform = `translateX(${translateValue}%)`;
    });
};

buttons.forEach((button) => {
    button.addEventListener("click", () => {
        button.dataset.button == "next" ? ++currSlide : --currSlide;

        if (currSlide > maxSlide) {
            currSlide = 0;
        } else if (currSlide < 0) {
            currSlide = maxSlide;
        }

        updateCarousel(currSlide);
    });
});

updateCarousel();

// Write your JavaScript code.
