

// Tema del sito 
const html = document.documentElement;
const toggle = document.getElementById('themeToggle');

// Carica il tema salvato
const savedTheme = localStorage.getItem('theme');
if (savedTheme) html.setAttribute('data-bs-theme', savedTheme);

toggle.addEventListener('click', () => {
    const currentTheme = html.getAttribute('data-bs-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    html.setAttribute('data-bs-theme', newTheme);
    localStorage.setItem('theme', newTheme);
});

