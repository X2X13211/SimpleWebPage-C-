document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.querySelector('form[action="/api/auth/login"]');
    const registrationForm = document.querySelector('form[action="/api/auth/register"]');

    //---ОБРАБОТКА ВХОДА---
    if (loginForm) {
        loginForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const usernameInput = document.getElementById('login_username');
            const passwordInput = document.getElementById('login_password');

            const username = usernameInput ? usernameInput.value : '';
            const password = passwordInput ? passwordInput.value : '';

            try {
                const response = await fetch('/api/auth/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ username, password })
                });

                if (response.ok) {
                    alert('Вы успешно вошли!');
                    window.location.href = '/dashboard.html';
                } else {
                    const errorText = await response.text();
                    alert('Ошибка входа: ' + (errorText || 'Неверный логин или пароль'));
                }
            } catch (error) {
                console.error('Ошибка сети:', error);
                alert('Не удалось связаться с сервером');
            }
        });
    }

    //---ОБРАБОТКА РЕГИСТРАЦИИ---
    if (registrationForm) {
        registrationForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const usernameInput = document.getElementById('username');
            const emailInput = document.getElementById('email');
            const passwordInput = document.getElementById('password');
            const passwordConfirmInput = document.getElementById('accept_password');

            const username = usernameInput ? usernameInput.value : '';
            const email = emailInput ? emailInput.value : '';
            const password = passwordInput ? passwordInput.value : '';
            const passwordConfirm = passwordConfirmInput ? passwordConfirmInput.value : '';

            if (password !== passwordConfirm) {
                alert('Пароли не совпадают!');
                return;
            }

            try {
                const response = await fetch('/api/auth/register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ username, email, password })
                });

                if (response.ok) {
                    alert('Регистрация прошла успешно!');
                    window.location.href = '/login_form.html';
                } else {
                    const errorText = await response.text();
                    alert('Ошибка регистрации: ' + (errorText || 'Попробуйте другие данные'));
                }
            } catch (error) {
                console.error('Ошибка сети:', error);
                alert('Не удалось связаться с сервером');
            }
        });
    }

    //---ПЕРЕХОДЫ МЕЖДУ СТРАНИЦАМИ---
    const redirectToRegBtn = document.getElementById('page_login_redirect_button');
    if (redirectToRegBtn) {
        redirectToRegBtn.addEventListener('click', () => {
            window.location.href = '/registation_form.html';
        });
    }

    const redirectToLoginBtn = document.getElementById('page_register_redirect_button');
    if (redirectToLoginBtn) {
        redirectToLoginBtn.addEventListener('click', () => {
            window.location.href = '/login_form.html';
        });
    }

});

