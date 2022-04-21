function fadePassMessage() {
    let message = document.getElementById("incorrectPass");
    message.innerText = "";
}

function fadeNickMessage() {
    let message = document.getElementById("incorrectNick");
    message.innerText = "";
}

function fadePassMessage2() {
    let message = document.getElementById("incorrectPass2");
    message.innerText = "";
}




const ratings = document.querySelectorAll('.rating');

if (ratings.length > 0) {
    initRatings();
}

function initRatings() {
    let ratingActive, ratingValue;
    for (let index = 0; index < ratings.length; index++) {
        const rating = ratings[index];
        initRating(rating);
    }
}

function initRating(rating) {
    initRatingVars(rating);

    setRatingActiveWidth();
}

function initRatingVars(rating) {
    ratingActive = rating.querySelector('.rating__active');
    ratingValue = rating.querySelector('.rating__value');
}

function setRatingActiveWidth(index = ratingValue.innerHTML) {
    const ratingActiveWidth = index / 0.05;
    ratingActive.style.width = `${ratingActiveWidth}%`;
}