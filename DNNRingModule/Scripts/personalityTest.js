document.addEventListener("DOMContentLoaded", function () {
    // ÁTIRÁNYÍTÁS A BEJELENTKEZÉSI OLDALRA
    const loginBtn = document.getElementById("MyLoginButton");
    if (loginBtn) {
        loginBtn.addEventListener("click", function (e) {
            e.preventDefault();
            const headerLogin = document.querySelector('.LoginLink');
            if (headerLogin) {
                headerLogin.click();
            } else {
                alert("Kérlek használd a fenti 'Bejelentkezés' menüpontot.");
            }
        });
    }
    // A RADIO-GOMBOS FORMOKNÁL MINDEN KÉRDÉSRE VAN VÁLASZ?
    const forms = document.querySelectorAll('form[action*="PersonalityTest"]');

    forms.forEach(form => {
        form.addEventListener("submit", function (e) {
            // A VISSZA GOMBOT NEM KELL VALIDÁLNI
            if (form.querySelector('button[type="submit"]').textContent === "⬅️ Vissza") {
                return;
            }

            const radios = form.querySelectorAll('input[type="radio"]');
            let isValid = true;
            const existingErrorMessages = form.querySelectorAll('.error-message');
            existingErrorMessages.forEach(errorMessage => errorMessage.remove());
            let firstUnansweredQuestion = null;

            radios.forEach(radio => {
                const questionName = radio.name;
                const selectedAnswer = form.querySelector(`input[name="${questionName}"]:checked`);
                const questionTitle = form.querySelector(`#${questionName}`);

                if (!selectedAnswer) {
                    isValid = false;
                    const existingError = questionTitle.querySelector('.error-message');
                    if (!existingError) {
                        const errorText = document.createElement('span');
                        errorText.className = 'error-message';
                        errorText.textContent = ' Válaszolj erre a kérdésre is!'; 
                        questionTitle.appendChild(errorText);
                    }
                    if (!firstUnansweredQuestion) {
                        firstUnansweredQuestion = questionTitle;
                    }
                }
                radio.addEventListener('change', function () {
                    const existingError = questionTitle.querySelector('.error-message');
                    if (existingError) {
                        existingError.remove();
                    }
                });
            });

            if (firstUnansweredQuestion) {
                firstUnansweredQuestion.scrollIntoView({behavior: 'smooth', block: 'center', inline: 'nearest'});
            }
            
            if (!isValid) {
                e.preventDefault();
            }
        });
    });

    //AMIKOR VISSZALÉPÜNK, ELMENTJÜK A "LEVEGŐBEN LÓGÓ" VÁLASZOKAT A MÁSODIK OLDALRÓL
    document.getElementById('visszaButton').addEventListener('click', function (event) {
        event.preventDefault();
        var secondForm = document.getElementById('secondForm');
        var visszaForm = document.getElementById('visszaForm');
        var answers = {
            q6: secondForm.querySelector('input[name="q6"]:checked') ? secondForm.querySelector('input[name="q6"]:checked').value : '',
            q7: secondForm.querySelector('input[name="q7"]:checked') ? secondForm.querySelector('input[name="q7"]:checked').value : '',
            q8: secondForm.querySelector('input[name="q8"]:checked') ? secondForm.querySelector('input[name="q8"]:checked').value : '',
            q9: secondForm.querySelector('input[name="q9"]:checked') ? secondForm.querySelector('input[name="q9"]:checked').value : '',
            q10: secondForm.querySelector('input[name="q10"]:checked') ? secondForm.querySelector('input[name="q10"]:checked').value : ''
        };

        visszaForm.querySelector('[name="q6"]').value = answers.q6;
        visszaForm.querySelector('[name="q7"]').value = answers.q7;
        visszaForm.querySelector('[name="q8"]').value = answers.q8;
        visszaForm.querySelector('[name="q9"]').value = answers.q9;
        visszaForm.querySelector('[name="q10"]').value = answers.q10;

        visszaForm.submit();
    });

    // 10 percenként meghívja ezt a method-ot, így nem törlődik a session
    setInterval(function () {
        $.get("PersonalityTest/PingSession");
    }, 10 * 60 * 1000);
});