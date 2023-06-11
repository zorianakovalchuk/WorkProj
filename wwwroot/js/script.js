window.addEventListener("scroll", function () {
    var defaultHeader = document.querySelector(".header--default");
    var scrolledHeader = document.querySelector(".header--scrolled");
    var scrollPosition = window.scrollY;

    if (scrollPosition > 0) {

        scrolledHeader.style.display = "block";
    } else {

        scrolledHeader.style.display = "none";
    }
});
var categories = document.querySelectorAll('.category');

categories.forEach(function (category) {
    var subcategories = category.querySelector('.subcategory');

    category.addEventListener('mouseenter', function () {
        category.classList.add('active');
    });

    category.addEventListener('mouseleave', function () {
        category.classList.remove('active');
    });
});
const icon = document.querySelector('.header__icon__basket'); 
const iconScrolled = document.querySelector('.header__icon__basket__scrolled'); 
const overlay = document.getElementById('overlay');
const popup = document.getElementById('popup');

icon.addEventListener('click', () => {
    overlay.style.display = 'block';
    popup.classList.add('open');
});

overlay.addEventListener('click', () => {
    overlay.style.display = 'none';
    popup.classList.remove('open');
});
const icon_selected = document.querySelector('.header__icon__selected');
const icon_selectedScrolled = document.querySelector('.header__icon__selected__scrolled');
const popup_selected = document.getElementById('popupSelected');

icon_selected.addEventListener('click', () => {
    overlay.style.display = 'block';
    popup_selected.classList.add('open');
});

overlay.addEventListener('click', () => {
    overlay.style.display = 'none';
    popup_selected.classList.remove('open');
});

function openModal() {
    var modalOverlay = document.getElementById("modalOverlay");
    modalOverlay.style.display = "block";
}

function closeModal() {
    var modalOverlay = document.getElementById("modalOverlay");
    modalOverlay.style.display = "none";
}

function changeTab(tab) {
    var loginTab = document.getElementById("loginTab");
    var registrationTab = document.getElementById("registrationTab");
    var loginContent = document.getElementById("loginContent");
    var registrationContent = document.getElementById("registrationContent");

    if (tab === "login") {
        loginTab.classList.add("active");
        registrationTab.classList.remove("active");
        loginContent.classList.remove("d-none");
        registrationContent.classList.add("d-none");
    } else if (tab === "registration") {
        loginTab.classList.remove("active");
        registrationTab.classList.add("active");
        loginContent.classList.add("d-none");
        registrationContent.classList.remove("d-none");
    }
}

// Закриття спливаючого вікна при кліку поза вікном
window.onclick = function (event) {
    var modalOverlay = document.getElementById("modalOverlay");
    if (event.target === modalOverlay) {
        closeModal();
    }
};

function changeBorderColor(input) {
    input.classList.add("focused"); // Додаємо клас "focused" до поля вводу
}

function restoreBorderColor(input) {
    input.classList.remove("focused"); // Видаляємо клас "focused" з поля вводу
}
$(document).ready(function () {
    var phoneInput = document.getElementById('phone');
    Inputmask("+38 (099) 999-99-99").mask(phoneInput);
});

iconScrolled.addEventListener('click', () => {
    overlay.style.display = 'block';
    popup.classList.add('open');
});

icon_selectedScrolled.addEventListener('click', () => {
    overlay.style.display = 'block';
    popup_selected.classList.add('open');
});

