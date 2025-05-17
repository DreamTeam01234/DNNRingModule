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

    // A VÁLASZTOTT GOMBOK STÍLUSAI HELYESEN JELENJENEK MEG
    document.querySelectorAll('.personalityTestForm input[type="radio"]').forEach(radio => {
        radio.addEventListener('change', function () {
            const questionName = this.name;
            document.querySelectorAll(`input[name="${questionName}"]`).forEach(radioButton => {
                const label = radioButton.closest('label');
                if (label) {
                    label.classList.remove('checked');
                }
            });
            this.closest('label').classList.add('checked');
        });
    });

    // FORMOKNÁL MINDEN KÉRDÉSRE VAN VÁLASZ?
    const forms = document.querySelectorAll('.personalityTestForm');

    forms.forEach(form => {
        form.addEventListener("submit", function (e) {
            const isPersonalityTestForm = form.classList.contains('personalityTestForm');

            if (isPersonalityTestForm) {
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
                    firstUnansweredQuestion.scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'nearest' });
                }

                if (!isValid) {
                    e.preventDefault();
                }
            }
        });
    });


    // 10 percenként meghívja ezt a method-ot, így nem törlődik a session
    setInterval(function () {
        $.get("PersonalityTest/PingSession");
    }, 10 * 60 * 1000);
});
