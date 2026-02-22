const form = document.getElementById('loginForm');
const username = document.getElementById('username');
const password = document.getElementById('password');

form.addEventListener('submit', (e) => {
    e.preventDefault();

    const testCredentials = [
        {username: 'testuser1', password: 'password1'},
        {username: 'admin', password: 'adminpass'}
    ];

    for (const cred of testCredentials) {
        if (cred.username === username.value && cred.password === password.value) {
            return window.location.href = '/homePage,js';
        }
    }
    alert('Invalid username or password');
});
