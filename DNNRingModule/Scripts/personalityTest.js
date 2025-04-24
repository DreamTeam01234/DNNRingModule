document.addEventListener("DOMContentLoaded", function () {
    // ÁTIRÁNYÍTÁS A BEJELENTKEZÉSI OLDALRA
    const loginBtn = document.getElementById("customLoginTrigger");
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
            const questionNames = new Set([...radios].map(r => r.name));
            let isValid = true;

            const errorMessage = form.querySelector('.error-message');
            if (errorMessage) {
                errorMessage.remove();
            }

            questionNames.forEach(name => {
                const selectedAnswer = form.querySelector(`input[name="${name}"]:checked`);
                if (!selectedAnswer) {
                    isValid = false;
                }
            });
            if (!isValid) {
                e.preventDefault();
                const errorText = document.createElement('span');
                //EZT MAJD CSS-BEN
                errorText.className = 'error-message';
                errorText.style.color = 'red';
                errorText.style.fontSize = '14px';
                errorText.textContent = 'Kérlek válaszolj minden kérdésre a folytatáshoz!';
                form.querySelector('button[type="submit"]').after(errorText);
            }
        });
    });
});